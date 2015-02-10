using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Thuraiya{
  
  public class Config{

      public static readonly Dictionary<string, Config> of = new Dictionary<string, Config> 
      { 
            { "main",        new Config(@"config\\config.txt") }, 
            { "f.contracts", new Config(@"config\\forms\\Contracts.txt") }, 
      };

      private List<string> _props;
      private readonly Dictionary<string,string> properties;
      private Config(string configFile) : base() {
          properties = new Dictionary<string, string>();
          _props = new List<string>();
        try{
          
          string[]lines = File.ReadAllText(configFile,Encoding.GetEncoding("windows-1256")).Split('\n');
            foreach(string line in lines){
              string[] pval = line.Split(':');
              properties[pval[0]] = pval[1].Trim();
              _props.Add(pval[0]);
            }
          
        }catch(Exception ex){
          string error = (DateTime.Now.ToString("yyyyMMddHHmmss")+":ERROR:"+ex.Message);
          MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
          Environment.Exit(0);
        }
      }

      public bool Has(string prop)
      {
          return properties.ContainsKey(prop);
      }
      public string Get(string prop){
        if(!Has(prop)) return prop;
        return properties[prop];
      }

      public List<string> props { get { return _props; } }
  }

}