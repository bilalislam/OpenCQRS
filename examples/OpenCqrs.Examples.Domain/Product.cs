﻿using System;
using OpenCqrs.Domain;
using OpenCqrs.Examples.Domain.Events;

namespace OpenCqrs.Examples.Domain
{
    public class Product : AggregateRoot
    {
        public string Title { get; private set; }

        public Product()
        {            
        }

        public Product(Guid id, string title) : base(id)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            // If you want the event to be dispatched to the service bus,
            // use ProductCreatedBusMessage instead of ProductCreated.
            // Remember to update the connection string in the ServiceBusConfiguration
            // section in the appsettings.json file.
            AddAndApplyEvent(new ProductCreated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        public void UpdateTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                throw new ApplicationException("Product title is required.");

            AddAndApplyEvent(new ProductTitleUpdated
            {
                AggregateRootId = Id,
                Title = title
            });
        }

        private void Apply(ProductCreated @event)
        {
            Id = @event.AggregateRootId;
            Title = @event.Title;
        }

        private void Apply(ProductCreatedBusMessage @event)
        {
            Id = @event.AggregateRootId;
            Title = @event.Title;
        }

        private void Apply(ProductTitleUpdated @event)
        {
            Title = @event.Title;
        }
    }
}
