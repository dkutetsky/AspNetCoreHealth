﻿// <copyright file="HealthCheckStartupFilter.cs" company="Allan Hardy">
// Copyright (c) Allan Hardy. All rights reserved.
// </copyright>

using System;
using App.Metrics.AspNetCore.Health.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace App.Metrics.AspNetCore.Health
{
    public class HealthCheckStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                // Verify if AddHealthChecks was done before calling UseHealthChecks
                // We use the HealthCheckMarkerService to make sure if all the services were added.
                // AppMetricsHealthServicesHelper.ThrowIfHealthChecksNotRegistered(hostBuilder.ApplicationServices);
                // TODO: make public in health
                // HealthServicesHelper.ThrowIfHealthChecksNotRegistered(app.ApplicationServices);

                var aspNetMetricsMiddlewareHealthChecksOptions = app.ApplicationServices.GetRequiredService<AppMetricsMiddlewareHealthChecksOptions>();

                if (aspNetMetricsMiddlewareHealthChecksOptions.HealthEndpointEnabled)
                {
                    app.UseMiddleware<HealthCheckEndpointMiddleware>();
                }

                next(app);
            };
        }
    }
}
