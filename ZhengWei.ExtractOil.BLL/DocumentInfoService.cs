using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhengWei.ExtractOil.IBLL;
using ZhengWeil.ExtractOil.Model;

namespace ZhengWei.ExtractOil.BLL
{
    public class DocumentInfoService : BaseService<DocumentInfo>, IDocumentInfoService
    {
        public override void SetCurrentDal()
        {
            CurrentDal = this.DbSession.DocumentInfoDal;
        }
    }
}
