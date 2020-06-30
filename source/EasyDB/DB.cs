//-----------------------------------------------------------------------
// <copyright file="DB.cs" company="The Public">
// Copyright 2020 The Public
//
// EasyDB C# is free software: you can redistribute it and/or modify 
// it under the terms of the GNU General Public License as published by the 
// Free Software Foundation, either version 3 of the License, or (at your 
// option) any later version.
//
// EasyDB C# is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of 
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU General 
// Public License for more details.
//
// You should have received a copy of the GNU General Public License 
// along with EasyDB c#.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------

namespace EasyDB
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    
    using Newtonsoft.Json;

    /// <summary>
    /// The DB class.
    /// </summary>
    public class DB
    {
        /// <summary>
        /// The base URL constant.
        /// </summary>
        private const String baseUrl = "https://app.easydb.io/database/";

        /// <summary>
        /// The database key.
        /// </summary>
        private readonly String database;

        /// <summary>
        /// The token key.
        /// </summary>
        private readonly String token;

        /// <summary>
        /// The URL.
        /// </summary>
        private readonly String url;

        /// <summary>
        /// The client.
        /// </summary>
        private readonly HttpClient client = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="DB"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="token">The token.</param>
        public DB(String database, String token)
        {
            this.database = database;
            this.token = token;

            this.client.DefaultRequestHeaders.Add("token", token);
            this.url = String.Format("{0}{1}", DB.baseUrl, database);
        }

        /// <summary>
        /// Executes a Put operation asynchronously.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public async Task PutAsync(String key, String value)
        {
            var json = this.ToJsonString(key, value);

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(this.url, data);

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Executes a Put operation.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public void Put(String key, String value)
        {
            this.PutAsync(key, value).Wait();
        }

        /// <summary>
        /// Executes a Get operation asynchronously.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<String> GetAsync(String key)
        {
            String getUrl = String.Format("{0}/{1}", url, key);
            return await client.GetStringAsync(getUrl);
        }

        /// <summary>
        /// Executes a Get operation.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String Get(String key)
        {
            return this.GetAsync(key).Result;
        }

        /// <summary>
        /// Gets all items in the database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<String> ListAsync()
        {
            return await client.GetStringAsync(url);
        }

        /// <summary>
        /// Gets all items in the database.
        /// </summary>
        /// <returns></returns>
        public String List()
        {
            return this.ListAsync().Result;
        }

        /// <summary>
        /// Converts the key and value to a JSON string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private String ToJsonString(String key, String value)
        {
            var dataItem = new EasyDBDataItem();
            dataItem.Key = key;
            dataItem.Value = value;            
            return JsonConvert.SerializeObject(dataItem);
        }
    }    
}
