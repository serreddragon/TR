﻿using Common.Model.ServiceBus;
using Common.ServiceBus.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.ServiceBus.Implementation
{
    public class ServiceBusSender : BaseServiceBus, IServiceBusSender
    {
        private readonly IServiceBusConfiguration _serviceBusConfiguration;

        public ServiceBusSender(IServiceBusConfiguration serviceBusConfiguration)
        {
            _serviceBusConfiguration = serviceBusConfiguration;
        }

        public async Task SendServiceBusMessages(IEnumerable<object> messageObjects)
        {
            await using var serviceBusSender = _serviceBusConfiguration.GetServiceBusSender();
            var messages = messageObjects.Select(s => SerializeToServiceBusMessage(s)).ToList();

            await serviceBusSender.SendMessagesAsync(messages);
        }

        public async Task SendAccountServiceBusMessage(IEnumerable<AccountServiceBusMessageObject> messageObjects)
        {
            await SendServiceBusMessages(messageObjects);
        }
    }
}
