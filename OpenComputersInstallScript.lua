local defaultConfigPastebinId = "M8EsUmRg";
local programPastebinId = "ntnP7M8P";

local shell = require("shell")
shell.execute('mkdir /lib')
shell.execute('wget -fq "https://raw.githubusercontent.com/rater193/OpenComputers-1.7.10-Base-Monitor/master/lib/json.lua" "/lib/json.lua"')

os.execute("mkdir BeeBreeder");
os.execute("pastebin get " .. defaultConfigPastebinId .. " BeeBreeder/config.json");
os.execute("pastebin get " .. programPastebinId .. " BeeBreeder/beeBreeder.lua -f");

local io = require("io")
local updateFile = io.open("BeeBreeder/update.lua","w")
updateFile:write("os.execute(\"pastebin get " .. programPastebinId .. " beeBreeder.lua -f\")")
updateFile:close()