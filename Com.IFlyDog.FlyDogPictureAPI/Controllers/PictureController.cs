using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogPictureAPI.Controllers
{
    /// <summary>
    /// 图片相关API
    /// </summary>
    public class PictureController : ApiController
    {
        private ILogger _logger = LogManager.GetLogger("PictureController");
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns>返回上传图片的url</returns>
        [HttpPost]
        public async Task<HttpResponseMessage> UploadImage([FromUri]string imagepath)
        {
            _logger.Info("1111111111111111111111111" + imagepath);
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            //string result = "";
            // 检查是否是 multipart/form-data
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                //InvestmentCommon.Log4NetHelper.Log.Error("不是有效的'form-data'类型");
                result.ResultType = IFlyDogResultType.Failed;
                result.Message = "不是有效的'form-data'类型";
                return new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "text/x-json") };
            }


            DateTime dt = DateTime.Now;
            string ImagePath = ConfigurationManager.AppSettings["ImageUrl"];
            string path = string.Format("/Image/{0}/", imagepath);
            string abtPath = HttpContext.Current.Server.MapPath(path);

            if (!Directory.Exists(abtPath))
            {
                Directory.CreateDirectory(abtPath);
            }


            string newFileName = "";
            //string ext = "";
            //string filePath = "";

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            HttpFileCollectionBase imgFiles = request.Files;
            await Task.Run(() =>
            {
                for (int i = 0; i < imgFiles.Count; i++)
                {
                    HttpPostedFileBase postedFile = imgFiles[i];
                    string fileName = postedFile.FileName;//完整的路径
                                                          //fileName = Path.GetFileName(postedFile.FileName); //获取到名称
                                                          //string fileExtension = Path.GetExtension(fileName);//文件的扩展名称
                    string type = fileName.Substring(fileName.LastIndexOf(".") + 1);    //类型  

                    newFileName = Guid.NewGuid().ToString("N") + "." + type;

                    if (postedFile.ContentLength > 0)
                        postedFile.SaveAs(abtPath + newFileName);
                }
            });


            result.ResultType = IFlyDogResultType.Success;
            result.Message = "图片上传成功";
            result.Data = ImagePath + path + newFileName;

            _logger.Info(result.Data);

            //result = Newtonsoft.Json.JsonConvert.SerializeObject(rList);
            return new HttpResponseMessage { Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(result), System.Text.Encoding.UTF8, "text/x-json") };
        }

        [HttpPost]
        public async Task<string> UploadPictureByCK(string CKEditorFuncNum, string CKEditor, string langCode)
        {
            //if (!Request.Content.IsMimeMultipartContent("form-data"))
            //{
            //    //InvestmentCommon.Log4NetHelper.Log.Error("不是有效的'form-data'类型");
            //    result.ResultType = IFlyDogResultType.Failed;
            //    result.Message = "不是有效的'form-data'类型";
            //    return result;
            //}


            DateTime dt = DateTime.Now;
            string ImagePath = ConfigurationManager.AppSettings["ImageUrl"];
            string path = string.Format("/Image/{0}/{1}{2}/", dt.Year, dt.Month.ToString().PadLeft(2, '0'), dt.Day.ToString().PadLeft(2, '0'));
            string abtPath = HttpContext.Current.Server.MapPath(path);

            if (!Directory.Exists(abtPath))
            {
                Directory.CreateDirectory(abtPath);
            }


            string newFileName = "";
            //string ext = "";
            //string filePath = "";

            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象
            HttpFileCollectionBase imgFiles = request.Files;
            await Task.Run(() =>
            {
                for (int i = 0; i < imgFiles.Count; i++)
                {
                    HttpPostedFileBase postedFile = imgFiles[i];
                    string fileName = postedFile.FileName;//完整的路径
                                                          //fileName = Path.GetFileName(postedFile.FileName); //获取到名称
                                                          //string fileExtension = Path.GetExtension(fileName);//文件的扩展名称
                    string type = fileName.Substring(fileName.LastIndexOf(".") + 1);    //类型  

                    newFileName = Guid.NewGuid().ToString("N") + "." + type;

                    if (postedFile.ContentLength > 0)
                        postedFile.SaveAs(abtPath + newFileName);
                }
            });

            var vMessage = string.Empty;
            _logger.Info(ImagePath + path + newFileName);
            string result = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + ImagePath + path + newFileName + "\", \"" + vMessage + "\");</script></body></html>";


            return result;
        }
    }
}
