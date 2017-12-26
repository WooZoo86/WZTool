using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Xml;
using System.Linq;
using System.Xml.Linq;


namespace WZToolLib.DAL.XML
{
    public enum TableType
    {
        DataFlow,
        DTC,
        Info
    }

    public interface IDataAccess
    {
        void SetXml(string path);

        DataRow GetRow(string strWhere);

        DataRow GetRow(string Flag, string tableName);

        DataRow GetRow(string columnname, string columnvalue, string tableName);

        DataRow GetRow(string Flag, int id, string tableName);

        DataRow GetRow(string Flag, string tableName, string tableAttributeName, string attributeName, string key);

        DataRow GetRow(int id, string TableName);

        DataRow GetRow(int id, string TableName, int xmlBase);

        DataRow GetRow(int flag, string tableName, string attributeName, int attriValue, int flagbase);

        List<string> GetAttributes(int id, string TableName, string attName);

        List<DataRow> GetRows(int id, string TableName);

        XmlNode GetXmlNode(string xml, string columnname, string columnvalue);

        DataSet GetSet();

        List<string> GetList(string colunmname);

        List<string> GetList(string colunmname, string tablename);

        List<string> GetList(string attributeName, string tablename, int way);

        List<string> GetListAccordingToKey(string colunmname, string key, string keyAttribute);

        List<string> GetListAccordingToKey(string colunmname, string tablename, string key, string keyAttribute);

        List<string> GetListAccordingToKey(string attributeName, string tablename, int way, string key, string keyAttribute);

        List<string> GetListAccordingToKey2(string colunmname, string tablename, string key, string keyAttribute);

        List<string> GetListAccordingToKey(string colunmname, string tablename, string key, string keyAttribute, TableType type);

        List<string> GetListAccordingToKey(string colunmname, string tablename, string key, int keyAttribute, TableType type);
    }

    public class XMLDecrypt
    {
        public static bool Decrypt(string config, ref XmlDocument xmlDoc)
        {
            return false;
        }
    }

    public class XMLDB : IDataAccess
    {
        public static IDataAccess GetInstance()
        {
            if (XMLDB.DBInstance == null)
            {
                lock (new object())
                {
                    XMLDB.DBInstance = new XMLDB();
                    return XMLDB.DBInstance;
                }
            }
            return XMLDB.DBInstance;
        }

        public void SetXml(string path)
        {
            this.XMLPath = path;
            if (!XMLDecrypt.Decrypt(path, ref this.xmlDoc))
            {
                this.xmlDoc.Load(path);
            }

            this.readerText = xmlDoc.InnerXml;
        }

        public DataRow GetRow(string colName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();

            DataTable source = dataSet.Tables[0];
            return (from result in source.AsEnumerable()
                    where result[colName].ToString() != null 
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(string colName, string colValue, bool fuzzy)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();

            DataTable source = dataSet.Tables[0];
            return (from result in source.AsEnumerable()
                    where result[colName].ToString().ToUpper() == colValue.ToUpper() 
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(string strWhere, string TableName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            
            DataTable source = dataSet.Tables[TableName];
            return (from result in source.AsEnumerable()
                    where result["FLAG"].ToString().PadLeft(8, '0').ToUpper() == strWhere.PadLeft(8, '0').ToUpper() || result["FLAG"].ToString().ToUpper() == strWhere.ToUpper()
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(string Flag, string tableName, string tableAttributeName, string attributeName, string key)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            DataTable source = dataSet.Tables[tableAttributeName];
            string attibuteResult = (from result in source.AsEnumerable()
                                     where key.ToUpper().Contains(result[attributeName].ToString().ToUpper())
                                     select result[0].ToString()).FirstOrDefault<string>();
            if (attibuteResult == null)
            {
                attibuteResult = "0";
            }
            DataTable seletedDS = dataSet.Tables[tableName];
            return (from result in seletedDS.AsEnumerable()
                    where result["FLAG"].ToString().PadLeft(10, '0').ToUpper() == Flag.PadLeft(10, '0').ToUpper() && result[seletedDS.Columns.Count - 1].ToString().Equals(attibuteResult)
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(int flag, string tableName, string attributeName, int attriValue, int flagbase)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            DataTable source = dataSet.Tables[tableName];
            return (from result in source.AsEnumerable()
                    where Convert.ToInt32(result["FLAG"].ToString(), flagbase) == flag && Convert.ToInt32(result[attributeName].ToString(), 16) == attriValue
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(int id, string TableName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            DataTable source = dataSet.Tables[TableName];
            return (from result in source.AsEnumerable()
                    where Convert.ToInt32(result["FLAG"].ToString(), 16) == id
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(int id, string TableName, int xmlBase)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            DataTable source = dataSet.Tables[TableName];
            return (from result in source.AsEnumerable()
                    where Convert.ToInt32(result["FLAG"].ToString(), xmlBase) == id
                    select result).FirstOrDefault<DataRow>();
        }

        public List<string> GetAttributes(int id, string TableName, string attName)
        {
            List<string> list = new List<string>();
            string xpath = "//" + TableName;

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                if (Convert.ToInt32(((XmlElement)xmlNodeList[i]).ChildNodes[1].InnerText.ToString(), 16).Equals(id))
                {
                    list.Add(((XmlElement)xmlNodeList[i]).GetAttribute(attName));
                }
            }
            return list;
        }

        public List<DataRow> GetRows(int id, string TableName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            DataTable source = dataSet.Tables[TableName];
            EnumerableRowCollection<DataRow> source2 = from result in source.AsEnumerable()
                                                       where Convert.ToInt32(result["FLAG"].ToString(), 16) == id
                                                       select result;
            return source2.ToList<DataRow>();
        }

        public DataSet GetSet()
        {
            DataSet dataSet = new DataSet();
            DataSet dataSet2 = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;
            dataSet2.ReadXml(xmlTextReader);
            xmlTextReader.Close();
            dataSet.Tables.Add(dataSet2.Tables[this.table_name]);
            return dataSet2;
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00002CAC File Offset: 0x00000EAC
        public List<string> GetList(string colunmname)
        {
            List<string> list = new List<string>();
            string xpath = "//" + this.table_name + "//" + colunmname;

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                list.Add(xmlNodeList[i].InnerXml);
            }
            return list;
        }

        public List<string> GetList(string colunmname, string tablename)
        {
            List<string> list = new List<string>();
            string xpath = "//" + tablename + "//" + colunmname;
            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                list.Add(xmlNodeList[i].InnerXml);
            }
            return list;
        }

        public List<string> GetList(string colunmname, string tablename, int way)
        {
            List<string> list = new List<string>();
            List<string> result;
            if (way == 0)
            {
                result = this.GetList(colunmname, tablename);
            }
            else
            {
                string xpath = "//" + tablename;
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    list.Add(((XmlElement)xmlNodeList[i]).GetAttribute(colunmname).ToString());
                }
                result = list;
            }
            return result;
        }

        public List<string> GetListAccordingToKey(string colunmname, string key, string keyAttribute)
        {
            List<string> list = new List<string>();
            string xpath = "//" + this.table_name + "//" + colunmname;

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                if (((XmlElement)xmlNodeList[i].ParentNode).GetAttribute(key) == keyAttribute)
                {
                    list.Add(xmlNodeList[i].InnerXml);
                }
            }
            return list;
        }

        public List<string> GetListAccordingToKey(string colunmname, string tablename, string key, string keyAttribute)
        {
            List<string> list = new List<string>();
            string xpath = "//" + tablename + "//" + colunmname;

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                if (keyAttribute != "")
                {
                    if (((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(key).Trim() != "")
                    {
                        if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(key)))
                        {
                            list.Add(xmlNodeList[i].InnerXml.Trim());
                        }
                    }
                }
                else if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(key)))
                {
                    list.Add(xmlNodeList[i].InnerXml.Trim());
                }
            }
            return list;
        }

        public List<string> GetListAccordingToKey2(string colunmname, string tablename, string key, string keyAttribute)
        {
            string[] array = key.Split(new char[]
            {
                ';'
            });
            List<string> list = new List<string>();
            string xpath = "//" + tablename + "//" + colunmname;

            XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (keyAttribute != "")
                    {
                        if (((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(array[j]).Trim() != "")
                        {
                            if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(array[j])))
                            {
                                list.Add(xmlNodeList[i].InnerXml);
                            }
                        }
                    }
                    else if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode.ParentNode).GetAttribute(array[j])))
                    {
                        list.Add(xmlNodeList[i].InnerXml);
                    }
                }
            }
            return list;
        }

        public List<string> GetListAccordingToKey(string colunmname, string tablename, string key, int keyAttribute, TableType type)
        {
            List<string> list = new List<string>();
            string xpath = "//" + tablename + "//" + colunmname;
            if (type.Equals(TableType.DataFlow))
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    int num = Convert.ToInt32(((XmlElement)xmlNodeList[i]).ParentNode.ChildNodes[1].InnerText, 16);
                    if (keyAttribute == num)
                    {
                        list.Add(xmlNodeList[i].InnerXml);
                    }
                }
            }
            return list;
        }

        public List<string> GetListAccordingToKey(string colunmname, string tablename, string key, string keyAttribute, TableType type)
        {
            List<string> list = new List<string>();
            string xpath = "//" + tablename + "//" + colunmname;
            if (type.Equals(TableType.DataFlow))
            {
                XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    if (keyAttribute.Equals(((XmlElement)xmlNodeList[i]).ParentNode.ChildNodes[1].InnerText.ToString()))
                    {
                        list.Add(xmlNodeList[i].InnerXml);
                    }
                }
            }
            return list;
        }

        public List<string> GetListAccordingToKey(string attributeName, string tablename, int way, string key, string keyAttribute)
        {
            List<string> list = new List<string>();
            List<string> result;
            if (way == 0)
            {
                result = this.GetList(attributeName, tablename);
            }
            else
            {
                if (way == 1)
                {
                    string xpath = "//" + tablename;
                    XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        if (keyAttribute != "")
                        {
                            if (((XmlElement)xmlNodeList[i].ParentNode).GetAttribute(key).Trim() != "")
                            {
                                if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode).GetAttribute(key)))
                                {
                                    list.Add(((XmlElement)xmlNodeList[i]).GetAttribute(attributeName).ToString());
                                }
                            }
                        }
                        else if (keyAttribute.Contains(((XmlElement)xmlNodeList[i].ParentNode).GetAttribute(key)))
                        {
                            list.Add(((XmlElement)xmlNodeList[i]).GetAttribute(attributeName).ToString());
                        }
                    }
                }
                else if (way == 2)
                {
                    string xpath = "//" + tablename;
                    XmlNodeList xmlNodeList = xmlDoc.SelectNodes(xpath);
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        if (keyAttribute.Contains(xmlNodeList[i].FirstChild.InnerXml))
                        {
                            list.Add(xmlNodeList[i].LastChild.InnerXml);
                        }
                    }
                }
                result = list;
            }
            return result;
        }

        public DataRow GetRow(string Flag, int type, string tableName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();

            DataTable source = dataSet.Tables[tableName];
            return (from result in source.AsEnumerable()
                    where result["FLAG"].ToString().PadLeft(8, '0').ToUpper() == Flag.PadLeft(8, '0').ToUpper() && Convert.ToInt32(result["TYPE"].ToString()) == type
                    select result).FirstOrDefault<DataRow>();
        }

        public DataRow GetRow(string columnname, string columnvalue, string tableName)
        {
            DataSet dataSet = new DataSet();
            XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(this.readerText));
            xmlTextReader.WhitespaceHandling = WhitespaceHandling.None;

            dataSet.ReadXml(xmlTextReader);
            xmlTextReader.Close();

            DataTable source = dataSet.Tables[tableName];
            return (from result in source.AsEnumerable()
                    where result[columnname].ToString().ToUpper() == columnvalue.ToUpper()
                    select result).FirstOrDefault<DataRow>();
        }

        public XmlNode GetXmlNode(string xml, string columnname, string columnvalue)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            foreach (object obj in xmlDocument.ChildNodes)
            {
                XmlNode xmlNode = (XmlNode)obj;
                foreach (object obj2 in xmlNode.ChildNodes)
                {
                    XmlNode xmlNode2 = (XmlNode)obj2;
                    if (xmlNode2.Name == columnname && xmlNode2.InnerText == columnvalue)
                    {
                        return xmlNode;
                    }
                }
            }
            return null;
        }

        private string XMLPath;

        private string table_name;

        private string readerText = "";

        protected static IDataAccess DBInstance = null;

        private XmlDocument xmlDoc = new XmlDocument();
    }
}
