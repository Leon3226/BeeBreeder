using BeeBreeder.Common.Data;
using BeeBreeder.WebAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Context = BeeBreeder.Data.Models.BeeBreederContext;

namespace BeeBreeder.Data.Providers
{
    public class ModsProvider : IModsProvider
    {
        private Mod[] _modsCache;
        private bool _cached = false;

        public Mod[] AllAvaliableMods()
        {
            if (!_cached)
            {
                using (var context = new Context())
                {
                    _modsCache = context.Mods.Select(x => new Mod {Id = x.Id, Name = x.Name  }).ToArray();
                }
            }
            return _modsCache;
        }
    }
}
