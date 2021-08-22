namespace ir.metrix.unity
{
    public class MetrixAttribution
    {
        public string acquisitionAd { get; set; }
        public string acquisitionAdSet { get; set; }
        public string acquisitionCampaign { get; set; }
        public string acquisitionSource { get; set; }
        public string attributionStatus { get; set; }

        public MetrixAttribution() {}

        public MetrixAttribution(string jsonString)
        {
            var jsonNode = JSON.Parse(jsonString);
            if (jsonNode == null)
            {
                return;
            }

            acquisitionAd = GetJsonString(jsonNode, "acquisitionAd");
            acquisitionAdSet = GetJsonString(jsonNode, "acquisitionAdSet");
            acquisitionCampaign = GetJsonString(jsonNode, "acquisitionCampaign");
            acquisitionSource = GetJsonString(jsonNode, "acquisitionSource");
            attributionStatus = GetJsonString(jsonNode, "attributionStatus");
        }

        private string GetJsonString(JSONNode node, string key)
        {
            if (node == null)
            {
                return null;
            }

            // Access value object and cast it to JSONData.
            var nodeValue = node[key] as JSONData;

            if (nodeValue == null)
            {
                return null;
            }

            if (nodeValue == "")
            {
                return null;
            }

            return nodeValue.Value;
        }
    }
}