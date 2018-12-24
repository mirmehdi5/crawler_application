using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;


namespace CrawlerApplication
{
    public partial class Index : System.Web.UI.Page
    {
        int MaxNewsSize =Convert.ToInt32(ConfigurationManager.AppSettings["MaxNewsSize"] != null ? Convert.ToString(ConfigurationManager.AppSettings["MaxNewsSize"]) : "25");
        String Rstring;
        String rootURL = "http://edition.cnn.com/";
        string name_in_news = "donald";
        List<string> finalLinks = new List<string>();
        protected void Page_Load(object sender, EventArgs e)
        {
            startfunction();
        }

        public void startfunction()
        {
            List<string> Homepage_Links = readpages(rootURL); //main list
            displaylinks(Homepage_Links);
            List<string> hreflinks = getpages(Homepage_Links);
            foreach (var item in hreflinks)
            {
                List<string> childlinks = readpages(item);
                displaylinks(childlinks);
                List<string> childhreflink = getpages(childlinks);
            }
        }
        public List<string> GetNewLinks(string content, string tag)
        {
            Regex regexLink = new Regex("<\\s*" + tag + "[^>]*>(.*?)<\\s*/\\s*" + tag + ">");//regex to get all the anchor tags
            List<string> newLinks = new List<string>();
            foreach (var match in regexLink.Matches(content))
            {
                if (!newLinks.Contains(match.ToString()))
                    newLinks.Add(match.ToString());
            }
            return newLinks;
        }

        public List<string> getpages(List<string> pagelinks)
        {
            List<string> Links = new List<string>();
            foreach (var link in pagelinks)
            {
                string temp = link;
                if (temp.Contains("href=\"/") && temp.ToLower().Contains(name_in_news))
                {
                    string[] split = temp.Split(new[] { "href=\"/" }, StringSplitOptions.None);//extract URL from the tag
                    string[] split1 = split[1].Split(new[] { "\"" }, StringSplitOptions.None);
                    temp = rootURL + split1[0];
                    Links.Add(temp);
                }
            }
            return Links;
        }

        public List<string> readpages(string URL)
        {
            WebRequest myWebRequest;
            WebResponse myWebResponse;
            myWebRequest = WebRequest.Create(URL);
            myWebResponse = myWebRequest.GetResponse();//Returns a response from an Internet resource
            Stream streamResponse = myWebResponse.GetResponseStream();//return the data stream from the internet and save it in the stream
            StreamReader sreader = new StreamReader(streamResponse);//reads the data stream
            Rstring = sreader.ReadToEnd();//reads it to the end
            List<string> aLinks = GetNewLinks(Rstring, "a");//gets the links only
            streamResponse.Close();
            sreader.Close();
            myWebResponse.Close();
            return aLinks;
        }

        public void displaylinks(List<string> linkList)
        {
            foreach (var link in linkList)
            {
                string temp = link;
                if (temp.Contains("href=\"/") && temp.ToLower().Contains(name_in_news))
                {
                    string[] split = temp.Split(new[] { "href=\"/" }, StringSplitOptions.None);//get URL for images
                    temp = split[0] + "href=\"" + rootURL + split[1];
                    string finalimageurls = string.Empty;
                    string finalcontentdesciptiontemp = string.Empty;
                    if (temp.Contains("src=\"//"))
                    {
                        string[] imagrurlstart = temp.Split(new[] { "src=\"//cdn.cnn.com" }, StringSplitOptions.None);
                        string[] imageurlends = imagrurlstart[1].Split(new[] { "\"" }, StringSplitOptions.None);
                        finalimageurls = "//cdn.cnn.com" + imageurlends[0];
                    }
                    else
                    {
                        finalimageurls = "/images/trump.jpg";
                    }
                    string[] finallistsplit = temp.Split(new[] { "href=\"" }, StringSplitOptions.None);
                    string[] finallistsplit1 = finallistsplit[1].Split(new[] { "\"" }, StringSplitOptions.None);//extract URL from the tag 
                    string finallisttemp = finallistsplit1[0];
                    if (temp.Contains("alt=\""))
                    {
                        string[] finalcontentdesciption = temp.Split(new[] { "alt=\"" }, StringSplitOptions.None);
                        string[] finalcontentdesciption1 = finalcontentdesciption[1].Split(new[] { ". " }, StringSplitOptions.None);
                        finalcontentdesciptiontemp = finalcontentdesciption1[0];
                    }
                    if (finalLinks.FindIndex(o => string.Equals(finallisttemp, o, StringComparison.OrdinalIgnoreCase)) < 0 && finalLinks.Count <= MaxNewsSize)
                    {
                        finalLinks.Add(finallisttemp);
                        var lnkMicrosoft = new HyperLink //create Hyperlink Tag
                        {
                            Text = temp.ToString(),
                            CssClass= ".img_class",
                            ImageUrl = finalimageurls,
                            NavigateUrl = finallisttemp
                        };
                        Panel_CNN.Controls.Add(lnkMicrosoft);

                        if (finalcontentdesciptiontemp == "" || finalcontentdesciptiontemp.Contains("<") || finalcontentdesciptiontemp.Contains(">"))
                            finalcontentdesciptiontemp = "Click to read this article!";
                        else
                            finalcontentdesciptiontemp = finalcontentdesciptiontemp + ".... READ MORE...";


                        var lnkMicrosofttext = new HyperLink
                        {
                            Text = finalcontentdesciptiontemp,
                            NavigateUrl = finallisttemp
                        };
                        Panel_CNN.Controls.Add(lnkMicrosofttext);
                        Panel_CNN.Controls.Add(new Literal() { Text = "<hr/>" });                 
                    }
                }
            }
        }
    }
}