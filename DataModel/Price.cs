//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Price
    {
        public int Id { get; set; }
        public Nullable<int> NewsId { get; set; }
        public Nullable<decimal> LPrice { get; set; }
        public Nullable<decimal> HPirce { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<decimal> Delta { get; set; }
        public Nullable<decimal> Spread { get; set; }
        public string Comment { get; set; }
        public string PriceUnit { get; set; }
        public string Token { get; set; }
        public int MarketCrmId { get; set; }
    
        public virtual News News { get; set; }
    }
}
