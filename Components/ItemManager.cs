/*
' Copyright (c) 2018 Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.Framework;
using Christoc.Modules.TestURL.Models;
using DotNetNuke.Common.Utilities;
using System.Linq;

namespace Christoc.Modules.TestURL.Components
{
    interface IItemManager
    {
        void CreateItem(Item t);
        void DeleteItem(int itemId, int moduleId);
        void DeleteItem(Item t);
        IEnumerable<Item> GetItems(int moduleId);
        Item GetItem(int itemId, int moduleId);
        void UpdateItem(Item t);
        IDictionary<int, string> GetItems();
    }

    class ItemManager : ServiceLocator<IItemManager, ItemManager>, IItemManager
    {
        public const string KEY = "ITEM_CACHETEST";
        public void CreateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Insert(t);
                DataCache.SetCache(KEY, null);

            }
        }

        public void DeleteItem(int itemId, int moduleId)
        {
            var t = GetItem(itemId, moduleId);
            DeleteItem(t);
            DataCache.SetCache(KEY, null);

        }

        public void DeleteItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Delete(t);
                DataCache.SetCache(KEY, null);

            }
        }

        public IEnumerable<Item> GetItems(int moduleId)
        {

            IEnumerable<Item> t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.Get(moduleId);
            }
            return t;
        }
        public IDictionary<int,string> GetItems()
        {
            IDictionary<int, string> _Data = DataCache.GetCache<IDictionary<int, string>>(KEY);

            if (_Data == null)
            {
                _Data = new Dictionary<int, string>();
                foreach (var item in GetItems(123))
                {
                    _Data[item.ItemId] = item.ItemName;
                }
                DataCache.SetCache(KEY, _Data);
                
            }

            return _Data;
            
        }

        public Item GetItem(int itemId, int moduleId)
        {
            Item t;
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                t = rep.GetById(itemId, moduleId);
            }
            return t;
        }

        public void UpdateItem(Item t)
        {
            using (IDataContext ctx = DataContext.Instance())
            {
                var rep = ctx.GetRepository<Item>();
                rep.Update(t);
                DataCache.SetCache(KEY, null);

            }
        }

        protected override System.Func<IItemManager> GetFactory()
        {
            return () => new ItemManager();
        }
    }
}
