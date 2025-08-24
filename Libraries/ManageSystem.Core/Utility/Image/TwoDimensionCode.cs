using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThoughtWorks.QRCode.Codec;

namespace ManageSystem.Core.Utility
{

    /// <summary>
    /// 二维码
    /// </summary>
    public class TwoDimensionCode
    {

        /// <summary>  
        /// 生成二维码图片  
        /// </summary>  
        /// <param name="value">要生成二维码的字符串</param>       
        /// <param name="size">二维码每个颗粒大小尺寸</param>  
        /// <returns>二维码图片</returns>  
        public static Bitmap CreateImgCode(string value, int size = 10)
        {
            try
            {
                //创建二维码生成类  
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                //设置编码模式  
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                //设置编码测量度  
                qrCodeEncoder.QRCodeScale = size;

                //设置编码版本  
                qrCodeEncoder.QRCodeVersion = 0;
                //设置编码错误纠正  
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;

                //生成二维码图片  
                System.Drawing.Bitmap image = qrCodeEncoder.Encode(value);

                return image;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 创建二维码并且保存文件
        /// </summary>
        /// <param name="value">二维码的内容</param>
        /// <param name="path">保存路径，绝对路径，例：D:/Content/Image/Code/ </param>
        /// <param name="saveFileName">要保存的文件名称，如果为空则根据GUID自动生成，文件名称不能带后缀，默认后缀为 png</param>
        /// <param name="size">二维码每个颗粒大小尺寸</param>
        /// <returns>返回文件的完整绝对路径，例如：D:/Content/Image/Code/test.png </returns>
        public static string CreateAndSaveCode(string value, string path, ref string saveFileName, int size = 10)
        {
            if (string.IsNullOrWhiteSpace(saveFileName))
                saveFileName = Guid.NewGuid().ToString().Replace("-", "");    //图片名称

            saveFileName += ".png";
            Bitmap image = CreateImgCode(value, size); //生成二维码图片
            string serverPath = path + saveFileName;
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(serverPath)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(serverPath));
            }
            image.Save(serverPath, System.Drawing.Imaging.ImageFormat.Png);
            image.Dispose();

            return path + saveFileName;
        }

    }
}
