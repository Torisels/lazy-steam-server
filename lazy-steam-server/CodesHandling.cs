using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lazy_steam_server
{
    class CodesHandling
    {
        private string _code;
        private string _hash;
        private const int KeySize = 256;

        public CodesHandling(string code, string hash = null)
        {
            _code = code;
            if (hash != null)
                _hash = hash;

        }

    }
}
