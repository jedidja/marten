﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using Marten.Linq;

namespace Marten.Events
{
    public static class EventStoreExtensions
    {
        public static T AggregateTo<T>(this IMartenQueryable<IEvent> queryable, T state = null) where T : class
        {
            var events = queryable.ToList();
            if (!events.Any())
            {
                return null;
            }

            var session = queryable.As<MartenLinqQueryable<IEvent>>().MartenSession;
            var aggregator = session.Options.Events.Projections.AggregatorFor<T>();

            var aggregate = aggregator.Build(queryable.ToList(), (IQuerySession)session, state);

            return aggregate;
        }

        public static async Task<T> AggregateToAsync<T>(this IMartenQueryable<IEvent> queryable, T state = null,
                                                        CancellationToken token = new ()) where T : class
        {
            var events = await queryable.ToListAsync(token);
            if (!events.Any())
            {
                return null;
            }

            var session = queryable.As<MartenLinqQueryable<IEvent>>().MartenSession;
            var aggregator = session.Options.Events.Projections.AggregatorFor<T>();

            var aggregate = await aggregator.BuildAsync(events, (IQuerySession)session, state, token);

            return aggregate;
        }

    }
}
