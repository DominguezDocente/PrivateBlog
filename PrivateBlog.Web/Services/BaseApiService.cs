using Newtonsoft.Json;
using PrivateBlog.Web.Core;
using PrivateBlog.Web.Helpers;
using System.Text;

namespace PrivateBlog.Web.Services
{
    public class BaseApiService
    {
        public async Task<Response<T>> GetAsync<T>(string url)
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Response<T> res = new Response<T>
                    {

                        IsSuccess = false,
                        Message = answer,
                    };

                    return res;
                }

                T result = JsonConvert.DeserializeObject<T>(answer);

                return ResponseHelper<T>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<T>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<T>> GetAsync<T>(string url, List<HeaderItem> headers)
        {
            try
            {

                HttpClient client = new HttpClient();

                foreach (HeaderItem header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Name, header.Value);
                }

                HttpResponseMessage response = await client.GetAsync(url);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {

                    Response<T> res = new Response<T>
                    {

                        IsSuccess = false,
                        Message = answer,
                    };

                    return res;

                }

                T result = JsonConvert.DeserializeObject<T>(answer);

                return ResponseHelper<T>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<T>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<T>> PostAsync<T>(string url, object content)
        {
            try
            {
                string req = JsonConvert.SerializeObject(content);

                StringContent body = new StringContent(req, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.PostAsync(url, body);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {

                    Response<T> res = new Response<T>
                    {

                        IsSuccess = false,
                        Message = answer,
                    };

                    return res;

                }

                T result = JsonConvert.DeserializeObject<T>(answer);

                return ResponseHelper<T>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<T>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<T>> PostAsync<T>(string url, object content, List<HeaderItem> headers)
        {
            try
            {
                string req = JsonConvert.SerializeObject(content);

                StringContent body = new StringContent(req, Encoding.UTF8, "application/json");

                HttpClient client = new HttpClient();

                foreach (HeaderItem header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Name, header.Value);
                }

                HttpResponseMessage response = await client.PostAsync(url, body);
                string answer = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {

                    Response<T> res = new Response<T>
                    {

                        IsSuccess = false,
                        Message = answer,
                    };

                    return res;

                }

                T result = JsonConvert.DeserializeObject<T>(answer);

                return ResponseHelper<T>.MakeResponseSuccess(result);
            }
            catch (Exception ex)
            {
                return ResponseHelper<T>.MakeResponseFail(ex);
            }
        }
    }
}
