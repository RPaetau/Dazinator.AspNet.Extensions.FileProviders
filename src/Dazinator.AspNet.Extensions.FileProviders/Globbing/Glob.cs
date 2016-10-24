﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dazinator.AspNet.Extensions.FileProviders.Globbing
{
    public class Glob
    {
        public string Pattern { get; private set; }

        private GlobNode _root;
        private Regex _regex;

        public Glob(string pattern, GlobOptions options = GlobOptions.None)
        {
            this.Pattern = pattern;
            if (options.HasFlag(GlobOptions.Compiled))
            {
                this.Compile();
            }
        }

        private void Compile()
        {
            if (_root != null)
                return;

            var parser = new Parser(this.Pattern);
            _root = parser.Parse();

            //TODO: this is basically cheating and probably not efficient but it works for now.
            _regex = new Regex(GlobToRegexVisitor.Process(_root), RegexOptions.Compiled);
        }

        public bool IsMatch(string input)
        {
            this.Compile();

            return _regex.IsMatch(input);
        }

        public static bool IsMatch(string input, string pattern)
        {
            return new Glob(pattern).IsMatch(input);
        }
    }
}
