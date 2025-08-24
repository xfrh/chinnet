using ManageSystem.Core.Utility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.User;
using Senparc.Weixin.MP.AppStore;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MessageHandlers;
using System;
using System.IO;

namespace ManageSystem.Mobile.Extensions
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<CustomMessageContext>
    {
        private string appId = "";
        private string appSecret = "";

        public CustomMessageHandler(Stream inputStream, PostModel postModel, int maxRecordCount = 0)
            : base(inputStream, postModel, maxRecordCount)
        {
           
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，

            if (!string.IsNullOrEmpty(postModel.AppId))
            {
                appId = postModel.AppId;//通过第三方开放平台发送过来的请求
            }

            //在指定条件下，不使用消息去重
            base.OmitRepeatedMessageFunc = requestMessage =>
            {
                var textRequestMessage = requestMessage as RequestMessageText;
                if (textRequestMessage != null && textRequestMessage.Content == "容错")
                {
                    return false;
                }
                return true;
            };
        }

   
        /// <summary>
        /// 会在所有消息处理方法（如OnTextRequest，OnVoiceRequest等）执行之前执行
        /// 这个过程中，我们可以把CancelExecute设为true，来中断后面所有方法的执行（包括OnExecuted
        /// </summary>
        public override void OnExecuting()
        {
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
            base.OnExecuting();
        }

        /// <summary>
        /// 执行完成以后会触发
        /// </summary>
        public override void OnExecuted()
        {
            base.OnExecuted();
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            try
            {
                if (requestMessage.Content == "爱的仪式感")
                {
                    var responseMessage1 = base.CreateResponseMessage<ResponseMessageText>();
                    responseMessage1.Content = "<a href='http://t.cn/E4GlW3u'>点击参加“穿越时空的幸福”</a>";
                    return responseMessage1;
                }

            
                FileLog.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "  文字推送：" + requestMessage.Content + "，系统回复："  );
                string openId = requestMessage.FromUserName;

                var responseMessage2 = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage2.Content = "";
                return responseMessage2;

            }
            catch (Exception ex)
            {
                var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = "发生错误：" + ex.Message;
                return responseMessage;
            }
        }

        ///// <summary>
        ///// 获取有关文章的微信回复对象
   
        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 处理小视频
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnShortVideoRequest(RequestMessageShortVideo requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 处理视频请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVideoRequest(RequestMessageVideo requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            throw new Exception();
        }

        /// <summary>
        /// 事件之扫码推事件且弹出“消息接收中”提示框(scancode_waitmsg)
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScancodeWaitmsgRequest(RequestMessageEvent_Scancode_Waitmsg requestMessage)
        {
            return base.OnEvent_ScancodeWaitmsgRequest(requestMessage);

            // try
            //  {
            //FileLog.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "扫码事件发生异常，开始：" + requestMessage.SerializeObject());

            //    string value = requestMessage.ScanCodeInfo.ScanResult;
            //    if (string.IsNullOrWhiteSpace(value))
            //        return base.OnEvent_ScancodeWaitmsgRequest(requestMessage);

            //    //标准结果：3192,2643  。2643是产品id
            //    string prouctId = value.Split(',')[1];

            //    var productView = this.productService.QueryView(int.Parse(prouctId));
            //    if (productView == null || productView.Id <= 0)
            //        return base.OnEvent_ScancodeWaitmsgRequest(requestMessage);

            //    var responseMessage = base.CreateResponseMessage<ResponseMessageNews>();

            //    responseMessage.Articles.Add(new Senparc.Weixin.MP.Entities.Article()
            //    {
            //        Title = "型号："+productView.Bianhao,
            //        Description ="点击查看产品信息",
            //        PicUrl = WebSettingService.GetWeixinUrl() + ProductExtensions.GetProductImage(productView.TupianLujing),
            //        Url = WebSettingService.GetWeixinUrl() + "/Product/Item?id=" + productView.Id
            //    });

            //    return responseMessage;
            //}
            //catch (Exception ex)
            //{
            //    FileLog.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "扫码事件发生异常，完整数据：" + requestMessage.SerializeObject() + "。错误内容：" + ex.ToString());
            //    return base.OnEvent_ScancodeWaitmsgRequest(requestMessage);
            // }
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            //FileLog.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "接受事件推送，开始：" + requestMessage.SerializeObject());


            IResponseMessageBase responseMessage = null;

            switch ((Event)requestMessage.Event)
            {
                case Event.ENTER:
                    break;
                case Event.LOCATION:
                    break;
                case Event.subscribe:
                    responseMessage = this.EventHandlerSubscribe(requestMessage);
                    break;
                case Event.unsubscribe:
                    responseMessage = this.EventHandlerUnSubscribe(requestMessage);
                    break;
                case Event.CLICK:
                    responseMessage = this.EventHandlerClick(requestMessage);
                    break;
                case Event.scan:

                    break;
                case Event.VIEW:
                    break;
                case Event.MASSSENDJOBFINISH:
                    break;
                case Event.TEMPLATESENDJOBFINISH:
                    break;
                case Event.scancode_push:
                    break;
                case Event.scancode_waitmsg:
                    responseMessage = OnEvent_ScancodeWaitmsgRequest(RequestMessage as RequestMessageEvent_Scancode_Waitmsg);
                    break;
                case Event.pic_sysphoto:
                    break;
                case Event.pic_photo_or_album:
                    break;
                case Event.pic_weixin:
                    break;
                case Event.location_select:
                    break;
                case Event.card_pass_check:
                    break;
                case Event.card_not_pass_check:
                    break;
                case Event.user_get_card:
                    break;
                case Event.user_del_card:
                    break;
                case Event.kf_create_session:
                    break;
                case Event.kf_close_session:
                    break;
                case Event.kf_switch_session:
                    break;
                case Event.poi_check_notify:
                    break;
                case Event.WifiConnected:
                    break;
                case Event.user_consume_card:
                    break;
                case Event.user_view_card:
                    break;
                case Event.user_enter_session_from_card:
                    break;
                case Event.merchant_order:
                    break;
                case Event.submit_membercard_user_info:
                    break;
                case Event.ShakearoundUserShake:
                    break;
                default:
                    responseMessage = base.OnEventRequest(requestMessage);
                    break;
            }
            if (responseMessage == null) responseMessage = base.OnEventRequest(requestMessage);

            return responseMessage;
        }

        /// <summary>
        /// 用户点击事件
        /// </summary>
        /// <returns></returns>
        private IResponseMessageBase EventHandlerClick(IRequestMessageEventBase requestMessage)
        {
            throw new Exception();

            //try
            //{
            //    RequestMessageEvent_Click message = requestMessage as RequestMessageEvent_Click;

            //    WeixinMenu model = this.weixinMenuService.QueryEntity(message.EventKey);

            //    switch ((WeixinMenuType)model.Type)
            //    {
            //        case WeixinMenuType.click:
            //            if (!string.IsNullOrWhiteSpace(model.Value))
            //            {
            //                string[] ids = model.Value.Split(',');
            //                var list = this.articleService.Query(m => ids.Contains(m.Id.ToString()) &&
            //                                                                m.Mark > 0 && m.State == (int)ArticleState.Normal).Take(8).
            //                                                               OrderBy(m => m.InsertTime).ToList();

            //                return this.GetArticleMessage(list, requestMessage);
            //            }
            //            break;
            //        case WeixinMenuType.view:
            //            break;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.systemLogService.Insert(ex, .Error);
            //}

            //return this.DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 用户关注事件
        /// </summary>
        /// <returns></returns>
        private IResponseMessageBase EventHandlerSubscribe(IRequestMessageEventBase requestMessage2)
        {
            RequestMessageEvent_Subscribe requestMessage = requestMessage2 as RequestMessageEvent_Subscribe;
            #region 用户关注公众号 数据操作
            try
            {
                UserInfoJson user = UserApi.Info(appId, requestMessage.FromUserName);
                SaveWeixinUser(requestMessage, user);
            }
            catch (Exception)
            {
            }

            #endregion
            var textResponseMessage = this.CreateResponseMessage<ResponseMessageText>();
            var viewEntity = "感谢您的关注";
            try
            {

            }
            catch (Exception)
            {

            }
            textResponseMessage.Content = viewEntity;
            return textResponseMessage;

            #region  目前不用


            ////     FileLog.WriteLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm") + "  关注昵称：" + requestMessage.FromUserName);
            //try
            //{
            //    UserInfoJson user = new UserInfoJson();
            //    try
            //    {
            //        //微信帐号类型不一样，可能没有权限获取用户的信息
            //        user = UserApi.Info(appId, requestMessage.FromUserName);
            //    }
            //    catch (Exception)
            //    {
            //    }

            //    #region  1、用户关注，将用户信息写入到数据库

            //    this.SaveWeixinUser(requestMessage, user);

            //    #endregion

            //    #region  2、将关注的用户保存成会员

            //    this.SaveMember(requestMessage, user, requestMessage.EventKey.ToLower());

            //    #endregion

            //    #region 3、推送内容给客户

            //    var entity = WebSettingService.GetSubscribe();
            //    //   Log4Helper.Error("关注事件，" + entity.SerializeObject());

            //    if (entity == null || string.IsNullOrWhiteSpace(entity.Value))
            //    {
            //        //后台没有设置相关关注推送内容，这里显示默认的推送信息
            //        var defaultResponseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //        defaultResponseMessage.Content = "欢迎您关注！";
            //        return defaultResponseMessage;
            //    }

            //    if (entity.Describe.Equals("2"))
            //    {
            //        //文章方式推送内容
            //        var articleList = this.articleService.Query(entity.Value).ToList();
            //        return this.GetArticleMessage(articleList, requestMessage);
            //    }

            //    //文字方式推送内容
            //    var textResponseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //    textResponseMessage.Content = entity.Value.Replace("<br>", "\r\n");

            //    return textResponseMessage;

            //    #endregion


            //}
            //catch (Exception ex)
            //{

            //    Log4Helper.Error("用户关注推送文章失败，" + ex.ToString());

            //    //发送异常推送默认的内容
            //    var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            //    responseMessage.Content = "欢迎您关注！";
            //    return responseMessage;
            //}

            #endregion
        }

        /// <summary>
        /// 用户关注事件，保存关注用户到数据库
        /// </summary>
        /// <param name="requestMessage"></param>
        private void SaveWeixinUser(IRequestMessageEventBase requestMessage, UserInfoJson user)
        {
            string openId = requestMessage.FromUserName;
      
        }

        /// <summary>
        /// 将关注的用户保存成会员
        /// </summary>
        /// <param name="requestMessage"></param>
        private void SaveMember(IRequestMessageEventBase requestMessage, UserInfoJson user, string eventKey)
        {
            //try
            //{
            //    string openId = requestMessage.FromUserName;
            //    var temp = this.memberService.QueryEntity(m => m.Mark > 0 && m.OpenId.Equals(openId));
            //    if (temp != null && temp.Id > 0)
            //    {
            //        //用户已经存在，只有是关注了然后取消关注再次关注会发生这种情况
            //        if (string.IsNullOrWhiteSpace(temp.NickName))
            //        {
            //            //用户是通过认证过来的，有可能没有微信的相关信息，比如昵称、头像等等
            //            temp.HeadImage = user.headimgurl;
            //            temp.LoginId = user.openid;
            //            temp.NickName = user.nickname;
            //            temp.Sex = user.sex;

            //            this.memberService.Update(temp);
            //        }
            //    }
            //    else
            //    {
            //        //新数据
            //        Member member = new Member()
            //        {
            //            LoginId = openId,
            //            Password = "888888",
            //            OpenId = openId,
            //            Phone = "",
            //            Email = "",
            //            Birthday = DateHelper.DefaultValue(),
            //            Status = (int)MemberStatus.Normal,
            //            Sex = (int)MemberSex.Unknown,
            //            Type = (int)MemberType.Beauty,
            //            InviteCode = MemberServiceExtensions.GetInviteCode(),
            //            InviteCodeImage = "",
            //            InputInviteCode = "",
            //            IntegralAmount = 0,
            //            Name = "",
            //            NickName = "",
            //            HeadImage = "",
            //            Describe = "",
            //            AttestationTime = DateHelper.DefaultValue(),
            //        };

            //        if (user != null && !string.IsNullOrWhiteSpace(user.nickname))
            //        {
            //            member.Name = user.nickname;
            //            member.NickName = user.nickname;
            //            member.HeadImage = user.headimgurl;
            //            member.Sex = user.sex;
            //        }

            //        //Log4Helper.Info("微信用户关注 - 会员信息，eventKey：" + eventKey+ "，temp="+ member.SerializeObject());


            //        this.memberService.Insert(member);
            //    }

            //}
            //catch (Exception ex)
            //{
            //    this.systemLogService.Insert("用户关注，保存会员到数据库失败", ex.ToString(), SystemLogLevel.Error, "", "ManageSystem.Weixin.Extensions.CustomMessageHandler.SaveMember()");
            //}

        }

        /// <summary>
        /// 用户取消关注事件
        /// </summary>
        /// <returns></returns>
        private IResponseMessageBase EventHandlerUnSubscribe(IRequestMessageEventBase requestMessage)
        {
            string openId = requestMessage.FromUserName;
           
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "取消关注成功！";

            return responseMessage;
        }

        /// <summary>
        /// 系统默认的回复内容
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            /* 所有没有被处理的消息会默认返回这里的结果，
            * 因此，如果想把整个微信请求委托出去（例如需要使用分布式或从其他服务器获取请求），
            * 只需要在这里统一发出委托请求，如：
            * var responseMessage = MessageAgent.RequestResponseMessage(agentUrl, agentToken, RequestDocument.ToString());
            * return responseMessage;
            */

            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "未能找到您所需要的内容";
            return responseMessage;
        }

    }
}