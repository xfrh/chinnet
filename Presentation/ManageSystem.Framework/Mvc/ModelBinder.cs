using System.Web.Mvc;

namespace ManageSystem.Framework.Mvc
{
    public class ModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext);
            if (model is ModelBinder)
            {
                ((ModelBinder)model).BindModel(controllerContext, bindingContext);
            }
            return model;
        }
    }
}
