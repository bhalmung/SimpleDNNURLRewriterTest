using System.Web.Mvc;
using Christoc.Modules.TestURL.Components;
using Christoc.Modules.TestURL.Models;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;

namespace Christoc.Modules.TestURL.Controllers
{
    [DnnHandleError]

    public class DetailsController : DnnController
    {
        public ActionResult Index(int itemId)
        {

            Item item = ItemManager.Instance.GetItem(itemId, 123);
            return View(item);
        }
    }
}