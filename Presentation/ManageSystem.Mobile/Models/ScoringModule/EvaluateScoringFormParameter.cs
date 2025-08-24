using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManageSystem.Mobile.Models.ScoringModule
{
    public sealed class EvaluateScoringFormParameter
    {
        public long id { get; set; }
        public string token { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }
        public string signature { get; set; }
        public Dictionary<string, List<EvaluateScoringItemFormParameter>> scoredata { get; set; }
    }

    public sealed class EvaluateScoringItemFormParameter
    {
        public string item { get; set; }
        public int sort { get; set; }
        public string value { get; set; }
        public double score { get; set; }
    }
}