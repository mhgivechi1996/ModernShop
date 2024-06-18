using Catalog.Domain.Core.AggregatesModel.ProductAggregate;
using Catalog.Domain.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernShopCommerce.Domain.Core.Catalogs.Products.Events
{
    public record class AddProductSendNotificationEvent : DomainEvent
    {
        public ProductId ProductId { get; init; }

        public AddProductSendNotificationEvent(ProductId productId)
        {
            ProductId = productId;
            AggregateId = productId.Value;
        }
    }
}
