using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CloudinaryApi
/// </summary>
public class CloudinaryApi
{
    public class results
    {
        public string public_id { get; set; }
        public int version { get; set; }
        public string signature { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
        public string resource_type { get; set; }
        public string created_at { get; set; }
        public List<object> tags { get; set; }
        public int bytes { get; set; }
        public string type { get; set; }
        public string etag { get; set; }
        public string url { get; set; }
        public string secure_url { get; set; }
        public string original_filename { get; set; }
    }
}