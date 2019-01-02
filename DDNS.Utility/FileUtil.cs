using DDNS.Entity.Tunnel;
using DDNS.Entity.Users;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DDNS.Utility
{
    public static class FileUtil
    {
        /// <summary>
        /// 读取内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static async Task<List<string>> ReadFile(string path)
        {
            var list = new List<string>();

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            var reader = new StreamReader(path, Encoding.Default);
            var line = string.Empty;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                list.Add(line);
            }

            reader.Close();
            return list;
        }

        /// <summary>
        /// 写入内容
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        private static void WriteFile(string path, List<string> list)
        {
            var fs = new FileStream(path, FileMode.OpenOrCreate);
            var writer = new StreamWriter(fs);

            foreach (var item in list)
            {
                writer.WriteLine(item);
            }

            writer.Flush();
            writer.Close();
            fs.Close();
        }

        /// <summary>
        /// 审核申请隧道,写入文本数据
        /// </summary>
        /// <param name="tunnel"></param>
        /// <param name="user"></param>
        /// <param name="path"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static async Task WriteTunnel(TunnelsEntity tunnel, UsersEntity user, string path, int port)
        {
            var list = new List<string>();
            var exist = false;
            var contentList = await ReadFile(path);
            if (contentList != null)
            {
                foreach (var item in contentList)
                {
                    var row = item.ToString().Split("|");
                    var authToken = row[0];
                    var subDomains = row[1];
                    var remotePorts = row[2];
                    var userName = row[3];
                    if (user.AuthToken == authToken)
                    {
                        subDomains += "," + tunnel.SubDomain;
                        remotePorts += "," + port;
                        exist = true;
                    }
                    var _item = authToken + "|" + subDomains + "|" + remotePorts + "|" + userName;
                    list.Add(_item);
                }
            }
            if (!exist)
            {
                list.Add(user.AuthToken + "|" + tunnel.SubDomain + "|" + port + "|" + user.UserName);
            }
            WriteFile(path, list);
        }

        /// <summary>
        /// 重置Token,写入文本数据
        /// </summary>
        /// <param name="path"></param>
        /// <param name="oldToken"></param>
        /// <param name="newToken"></param>
        /// <returns></returns>
        public static async Task ResetUserToken(string path, string oldToken, string newToken)
        {
            var list = new List<string>();

            var contentList = await ReadFile(path);
            if (contentList != null)
            {
                foreach (var item in contentList)
                {
                    var row = item.ToString().Split("|");
                    var authToken = row[0];
                    var subDomains = row[1];
                    var remotePorts = row[2];
                    var userName = row[3];

                    if (authToken == oldToken)
                    {
                        authToken = newToken;
                    }
                    var _item = authToken + "|" + subDomains + "|" + remotePorts + "|" + userName;
                    list.Add(_item);
                }
            }

            WriteFile(path, list);
        }
    }
}