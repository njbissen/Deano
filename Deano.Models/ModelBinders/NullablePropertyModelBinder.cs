using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AcumenRegistry.Models.ModelBinders
{
    public class NullablePropertyModelBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            object value = base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                if (value == null)
                {
                    return value += "";
                }
            }

            return value;
        }

    }
}
