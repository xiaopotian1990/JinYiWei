using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class PictureController : Controller
    {
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns>返回上传图片的url</returns>
        [HttpPost]
        public async Task<string> UploadImage(string imagepath)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();

            DateTime dt = DateTime.Now;
            string ImagePath = ConfigurationManager.AppSettings["ImageUrl"];
            string path = string.Format("/Image/{0}/", imagepath);
            string abtPath = Server.MapPath(path);

            if (!Directory.Exists(abtPath))
            {
                Directory.CreateDirectory(abtPath);
            }


            string newFileName = "";
            await Task.Run(() =>
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase postedFile = Request.Files[i];
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


            //result = Newtonsoft.Json.JsonConvert.SerializeObject(rList);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 批量上传图片
        /// </summary>
        /// <param name="imagepath"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BatchUploadImage(string imagepath)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<BatchImage>>();

            DateTime dt = DateTime.Now;
            string ImagePath = ConfigurationManager.AppSettings["ImageUrl"];
            string path = string.Format("/Image/{0}/", imagepath);
            string abtPath = Server.MapPath(path);

            if (!Directory.Exists(abtPath))
            {
                Directory.CreateDirectory(abtPath);
            }

            List<BatchImage> urlList = new List<BatchImage>();

            await Task.Run(() =>
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    BatchImage temp = new BatchImage();
                    string newFileName = "";
                    HttpPostedFileBase postedFile = Request.Files[i];
                    string fileName = postedFile.FileName;//完整的路径
                                                          //fileName = Path.GetFileName(postedFile.FileName); //获取到名称
                                                          //string fileExtension = Path.GetExtension(fileName);//文件的扩展名称
                    string type = fileName.Substring(fileName.LastIndexOf(".") + 1);    //类型  

                    newFileName = Guid.NewGuid().ToString("N") + "." + type;

                    if (postedFile.ContentLength > 0)
                        postedFile.SaveAs(abtPath + newFileName);

                    temp.BigImage = ImagePath + path + newFileName;
                    temp.ReducedImage = ImagePath + path + GetReducedImage(50, 50, abtPath, newFileName);
                    urlList.Add(temp);
                }
            });


            result.ResultType = IFlyDogResultType.Success;
            result.Message = "图片上传成功";
            result.Data = urlList;


            //result = Newtonsoft.Json.JsonConvert.SerializeObject(rList);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        /// <summary>
        /// 生成缩略图重载方法，返回缩略图的Image对象
        /// </summary>
        /// <param name="width">缩略图的宽度</param>
        /// <param name="height">缩略图的高度</param>
        /// <param name="imageFrom">原Image对象</param>
        /// <returns>缩略图的Image对象</returns>
        public string GetReducedImage(int width, int height, string abtPath, string newFileName)
        {
            Image imageFrom = Image.FromFile(abtPath + newFileName);
            // 源图宽度及高度 
            int imageFromWidth = imageFrom.Width;
            int imageFromHeight = imageFrom.Height;
            try
            {
                // 生成的缩略图实际宽度及高度.如果指定的高和宽比原图大，则返回原图；否则按照指定高宽生成图片
                if (width >= imageFromWidth && height >= imageFromHeight)
                {
                    return abtPath + newFileName;
                }
                else
                {
                    Image.GetThumbnailImageAbort callb = new Image.GetThumbnailImageAbort(() => { return false; });
                    //调用Image对象自带的GetThumbnailImage()进行图片缩略
                    Image reducedImage = imageFrom.GetThumbnailImage(width, height, callb, IntPtr.Zero);
                    string filename = Guid.NewGuid().ToString("N") + ".png";
                    //将图片以指定的格式保存到到指定的位置
                    reducedImage.Save(abtPath + filename, ImageFormat.Png);
                    return filename;
                }
            }
            catch (Exception)
            {
                //抛出异常
                throw new Exception("转换失败，请重试！");
            }
        }
    }
}