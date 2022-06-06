
local configFileName = "config.json";
local json = require("json");

function loadConfig()
  local file = io.open(configFileName);
  if (file) then
      local content = file:read("*all");
      file:close();
      debugPrint(content);
      return json.decode(content);
  end
end

local configuration = loadConfig();
local internet = require('internet');
local handle;
local component = require('component');

local reconnectIntervals = configuration.reconnectIntervalsSeconds;
local hostIp = configuration.hostIp;
local hostPort = configuration.hostPort;
local debugging = true;

function pong()
    return 'pong';
end

function printMessage(string)
    return print(string);
end

function saveConfig()
    local file = io.open(configFileName, 'w');
    if (file) then
        debugPrint(json.encode(configuration));
        file:write(json.encode(configuration))
        file:close();
    end
end

function connectToHost()
    debugPrint('Connecting to host...');
    handle = internet.open(hostIp, hostPort); -- use config
end

function allTransposers()
    return component.list('transposer');
end

function identifier()
    return configuration["apiaryIdentifier"];
end

function setIdentifier(identifier)
    configuration["apiaryIdentifier"] = identifier;
    saveConfig();
end

function split(inputstr, sep)
    if sep == nil then
        sep = "%s"
    end
    local t = {}
    for str in string.gmatch(inputstr, "([^" .. sep .. "]+)") do
        table.insert(t, str)
    end
    return t
end

function transposerInventories(transposerAdress)
    if (transposerAdress == nil) then
        return ''
    end
    local trnspsr = component.proxy(component.get(transposerAdress))
    local inventories = {}
    for side = 0, 5 do
        local size = trnspsr.getInventorySize(side);
        local name = trnspsr.getInventoryName(side);
        inventories[tostring(side)] = {
            size = size,
            name = name
        }
    end
    return inventories;
end

function getItems(transposerAdress, side)
    if (transposerAdress == nil or side == nil or tonumber(side) == nil or tonumber(side) < 0 or tonumber(side) > 5) then
        return ''
    end
    local trnspsr = component.proxy(component.get(transposerAdress))
    if (trnspsr == nil) then
        return ''
    end
    local items = {}
    local stacks = trnspsr.getAllStacks(tonumber(side));
    if (stacks == nil) then
        return ''
    end

    local place = 1;
    for item in stacks do
        items[place] = item;
        place = place + 1;
    end

    return items;
end

function moveItem(transposerAdress, firstSide, firstSlot, secondSide, secondSlot, count)
    -- TODO: add cache... maybe
    local fSide = tonumber(firstSide);
    local sSide = tonumber(secondSide);
    local fSlot = tonumber(firstSlot);
    local sSlot = tonumber(secondSlot);
    count = tonumber(count);

    if (transposerAdress == nil or fSide == nil or fSide < 0 or fSide > 5 or sSide == nil or sSide < 0 or sSide > 5 or
        count == nil) then
        return ''
    end

    local transposer = component.proxy(component.get(transposerAdress))

    if (transposer == nil) then
        return ''
    end

    return transposer.transferItem(fSide, sSide, count, fSlot, sSlot);

end

function isAnalyzedBee(item)
    return item.individual ~= nil and item.individual.type == 'bee' and item.individual.isAnalyzed;
end

function debugPrint(debugString)
    if (debugging) then
        print(debugString);
    end
end

function debugWrite(debugString)
    if (debugging) then
        io.write(debugString);
    end
end

local choiseTable = {
    ['transposers'] = allTransposers,
    ['inventories'] = transposerInventories,
    ['items'] = getItems,
    ['move'] = moveItem,
    ['identifier'] = identifier,
    ['ping'] = pong,
    ['setIdentifier'] = setIdentifier,
    ['print'] = printMessage
}

saveConfig();
connectToHost();

while (1) do
    local data = handle:read();
    if (data == nil) then
        debugPrint('A connection error occured. Trying to reconnect in ' .. reconnectIntervals .. ' seconds')
        os.sleep(reconnectIntervals);
        connectToHost();
    else
        debugPrint(data);
        if (data == 'exit') then
            handle:close()
            break
        end

        local separated = split(data);
        local comand = separated[1];
        local args = separated;
        table.remove(args, 1);

        local response = 'No function found';
        local func = choiseTable[comand];
        if (func ~= nil) then
            debugPrint('   selected ' .. comand);
            local passed, result = pcall(func, table.unpack(args));
            if (not passed) then
                response = 'error: ' .. result;
                debugPrint('An error occured!');
                debugPrint(result)
            else
                response = json.encode(result)
            end
        end
        handle:write(response);
    end
end
