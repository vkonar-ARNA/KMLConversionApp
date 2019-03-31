using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KMLExportApp
{
    class Program
    {
        static int Main(string[] args)
        {
            //ToDo
            //Take user input for input kml file and location of where to save the file
            /*Read kml File*/
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\psube\\Desktop\\TestOps\\ToBeConverted1.kml");
            
            XmlNode root = doc.DocumentElement.FirstChild;

            
            String coordinates = root.LastChild.LastChild.LastChild.InnerText;
            // when coordinates! equal to empty or null
            if (coordinates != null || coordinates.Length != 0)
            {      
                //change from meters to feet
                string[] waypoints = coordinates.Split(' ');
                List<string> newWayPoints=new List<string>();
                List<string> changedAltitudes = new List<string>();

                foreach (string wp in waypoints)
                {
                    string[] fields = wp.Split(',');
                    double altitude = Double.Parse(fields[2]);
                    fields[2] = ((double)(altitude * 3.28084)).ToString();
                    changedAltitudes.Add(fields[2]);

                    string entry = fields[0] + "," + fields[1] + "," + fields[2];
                    newWayPoints.Add(entry); 
                }
                if (newWayPoints.Count > 0)
                {
                    Console.WriteLine("Coordinates changed to feet....\n\r Coordinates:" + coordinates + "");
                    CreateKML(newWayPoints,changedAltitudes);
                }
                else { Console.WriteLine("No coordinates found KML not created"); }
               
            }
            else
            {
                Console.WriteLine("check the imported File.Coordinates field is wither null or empty");
            }
            return 0;
        }

        private static void CreateKML(List<string> newWayPoints, List<string> changedAltitudes)
        {
            XmlDocument xDoc = new XmlDocument();
            XmlDeclaration xDec = xDoc.CreateXmlDeclaration("1.0", "utf-16", null);

            XmlElement rootNode = xDoc.CreateElement("kml");
            rootNode.SetAttribute("xmlns", @"http://www.opengis.net/kml/2.2");
            xDoc.InsertBefore(xDec, xDoc.DocumentElement);
            xDoc.AppendChild(rootNode);
            XmlElement docNode = xDoc.CreateElement("Document");
            rootNode.AppendChild(docNode);
           
            
            for (int i=0;i<newWayPoints.Count;i++)
            {
                XmlElement placemarkNode = xDoc.CreateElement("Placemark");
                
                XmlElement nameNode = xDoc.CreateElement("name");
                string nameTextStr = "";
                if (i == 0)
                {
                    nameTextStr = "WP H Alt: "+changedAltitudes.ElementAt(i);
                }
                else
                {
                    nameTextStr = "WP " + i + " Alt: " + changedAltitudes.ElementAt(i);
                }
                nameNode.InnerText = nameTextStr;
                //XmlText nameText = xDoc.CreateTextNode(""+nameTextStr);               
                    
                XmlElement pointNode = xDoc.CreateElement("Point");
                XmlElement altitudeModeNode = xDoc.CreateElement("altitudeMode");
                // XmlText altitudeModeText = xDoc.CreateTextNode("absolute");
                altitudeModeNode.InnerText = "absolute";
                XmlElement coordinatesNode = xDoc.CreateElement("coordinates");
                // XmlText coordinatesNodeText = xDoc.CreateTextNode(newWayPoints.ElementAtOrDefault(i));
                coordinatesNode.InnerText = newWayPoints.ElementAtOrDefault(i);
                docNode.AppendChild(placemarkNode);
                placemarkNode.AppendChild(nameNode);
               // placemarkNode.AppendChild(nameText);

                placemarkNode.AppendChild(pointNode);
                pointNode.AppendChild(altitudeModeNode);
              //  pointNode.AppendChild(altitudeModeText);

                pointNode.AppendChild(coordinatesNode);
               // pointNode.AppendChild(coordinatesNodeText);
            }

            xDoc.Save("C: \\Users\\psube\\Desktop\\TestOps\\CodeCreatedXML\\new.kml");
            Console.WriteLine("");
            //handle if file already exits
        }
    }
}
