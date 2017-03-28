using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Sangmado.Inka.Extensions;
using Sangmado.Inka.Logging;

namespace Sangmado.Fida.Messaging
{
    public class BidirectionalSubscriber
    {
        private ILog _log = Logger.Get<BidirectionalSubscriber>();

        private bool _countable = true;
        // Resource --> [Subscriber, Resource, Owner]
        private ConcurrentDictionary<string, ConcurrentDictionary<int, Subscription>> _resources = new ConcurrentDictionary<string, ConcurrentDictionary<int, Subscription>>();
        // Subscriber --> [Subscriber, Resource, Owner]
        private ConcurrentDictionary<string, ConcurrentDictionary<int, Subscription>> _subscribers = new ConcurrentDictionary<string, ConcurrentDictionary<int, Subscription>>();

        public BidirectionalSubscriber()
            : this(false)
        {
        }

        public BidirectionalSubscriber(bool countable)
        {
            _countable = countable;
        }

        public void Subscribe(string subscriber, string resource)
        {
            if (subscriber.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            Subscribe(subscriber, resource, string.Empty);
        }

        public void Subscribe(string subscriber, string resource, string owner)
        {
            if (subscriber.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            Subscribe(new Subscription(subscriber, resource, owner));
        }

        public void Subscribe(Subscription subscription)
        {
            if (subscription == null) return;
            if (subscription.Subscriber.IsNullOrEmpty()) return;
            if (subscription.Resource.IsNullOrEmpty()) return;

            if (!_resources.ContainsKey(subscription.Resource))
                _resources.Add(subscription.Resource, new ConcurrentDictionary<int, Subscription>());
            if (!_subscribers.ContainsKey(subscription.Subscriber))
                _subscribers.Add(subscription.Subscriber, new ConcurrentDictionary<int, Subscription>());

            if (!_resources[subscription.Resource].ContainsKey(subscription.GetHashCode()))
                _resources[subscription.Resource].Add(subscription.GetHashCode(), subscription);
            if (!_subscribers[subscription.Subscriber].ContainsKey(subscription.GetHashCode()))
                _subscribers[subscription.Subscriber].Add(subscription.GetHashCode(), subscription);

            subscription = _resources[subscription.Resource][subscription.GetHashCode()];

            if (_countable)
            {
                subscription.Count = subscription.Count + 1;
            }
//#if DEBUG
//            _log.Debug(string.Format("Subscribe, [{0}].", subscription));
//#endif
        }

        public void Unsubscribe(string subscriber, string resource)
        {
            if (subscriber.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            Unsubscribe(subscriber, resource, string.Empty);
        }

        public void Unsubscribe(string subscriber, string resource, string owner)
        {
            if (subscriber.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            Unsubscribe(new Subscription(subscriber, resource, owner));
        }

        public void Unsubscribe(Subscription subscription)
        {
            if (subscription == null) return;
            if (subscription.Subscriber.IsNullOrEmpty()) return;
            if (subscription.Resource.IsNullOrEmpty()) return;

            if (!_resources.ContainsKey(subscription.Resource)) return;
            if (!_subscribers.ContainsKey(subscription.Subscriber)) return;

            subscription = _resources[subscription.Resource][subscription.GetHashCode()];

            if (_countable)
            {
                subscription.Count = subscription.Count - 1;
            }

            //_log.Debug(string.Format("Unsubscribe, Decreased, [{0}].", subscription));

            if (subscription.Count <= 0)
            {
                _resources[subscription.Resource].Remove(subscription.GetHashCode());
                _subscribers[subscription.Subscriber].Remove(subscription.GetHashCode());
                //_log.Debug(string.Format("Unsubscribe, Removed Item, [{0}].", subscription));
            }

            if (_resources[subscription.Resource].Count <= 0)
            {
                _resources.Remove(subscription.Resource);
                //_log.Debug(string.Format("Unsubscribe, Removed Resource, [{0}].", subscription.Resource));
            }
            if (_subscribers[subscription.Subscriber].Count <= 0)
            {
                _subscribers.Remove(subscription.Subscriber);
                //_log.Debug(string.Format("Unsubscribe, Removed Subscriber, [{0}].", subscription.Subscriber));
            }

//#if DEBUG
//            if (_resources.ContainsKey(subscription.Resource))
//            {
//                foreach (var item in _resources[subscription.Resource])
//                {
//                    _log.Debug(string.Format("Unsubscribe, Existing Subscription, [{0}].", item.Value));
//                }
//            }
//#endif
        }

        public void Unsubscribe(string subscriber)
        {
            if (subscriber.IsNullOrEmpty()) return;

            var subscriptions = GetSubscriptionsBySubscriber(subscriber).ToList();
            foreach (var subscription in subscriptions)
            {
                Unsubscribe(subscription);
            }
        }

        public void UnsubscribeStartsWith(string subscriberPrefix, string resource)
        {
            UnsubscribeStartsWith(subscriberPrefix, resource, string.Empty);
        }

        public void UnsubscribeStartsWith(string subscriberPrefix, string resource, string owner)
        {
            if (subscriberPrefix.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            if (!_resources.ContainsKey(resource)) return;

            var subscriptions = _resources[resource].Values.Where(w => w.Subscriber.StartsWith(subscriberPrefix)).ToList();
            foreach (var subscription in subscriptions)
            {
                Unsubscribe(subscription);
//#if DEBUG
//                if (_resources.ContainsKey(subscription.Resource))
//                {
//                    foreach (var item in _resources[subscription.Resource])
//                    {
//                        _log.Debug(string.Format("UnsubscribeStartsWith, Existing Subscription, [{0}].", item.Value));
//                    }
//                }
//#endif
            }
        }

        public void UnsubscribeEndsWith(string subscriberSuffix, string resource)
        {
            UnsubscribeEndsWith(subscriberSuffix, resource, string.Empty);
        }

        public void UnsubscribeEndsWith(string subscriberSuffix, string resource, string owner)
        {
            if (subscriberSuffix.IsNullOrEmpty()) return;
            if (resource.IsNullOrEmpty()) return;

            if (!_resources.ContainsKey(resource)) return;

            var subscriptions = _resources[resource].Values.Where(w => w.Subscriber.EndsWith(subscriberSuffix)).ToList();
            foreach (var subscription in subscriptions)
            {
                Unsubscribe(subscription);
//#if DEBUG
//                if (_resources.ContainsKey(subscription.Resource))
//                {
//                    foreach (var item in _resources[subscription.Resource])
//                    {
//                        _log.Debug(string.Format("UnsubscribeEndsWith, Existing Subscription, [{0}].", item.Value));
//                    }
//                }
//#endif
            }
        }

        public IEnumerable<Subscription> GetSubscriptionsBySubscriber(string subscriber)
        {
            var empty = new List<Subscription>();
            if (subscriber.IsNullOrEmpty()) return empty;

            if (!_subscribers.ContainsKey(subscriber)) return empty;
            return _subscribers[subscriber].Values.ToList();
        }

        public IEnumerable<Subscription> GetSubscriptionsBySubscriberStartsWith(string subscriberPrefix)
        {
            var empty = new List<Subscription>();
            if (subscriberPrefix.IsNullOrEmpty()) return empty;

            return _resources.SelectMany(i => i.Value).Select(m => m.Value).Where(k => k.Subscriber.StartsWith(subscriberPrefix)).ToList();
        }

        public IEnumerable<Subscription> GetSubscriptionsBySubscriberEndsWith(string subscriberSuffix)
        {
            var empty = new List<Subscription>();
            if (subscriberSuffix.IsNullOrEmpty()) return empty;

            return _resources.SelectMany(i => i.Value).Select(m => m.Value).Where(k => k.Subscriber.EndsWith(subscriberSuffix)).ToList();
        }

        public IEnumerable<Subscription> GetSubscriptionsByResource(string resource)
        {
            var empty = new List<Subscription>();
            if (resource.IsNullOrEmpty()) return empty;

            if (!_resources.ContainsKey(resource)) return empty;
            return _resources[resource].Values.ToList();
        }

        public IEnumerable<string> GetSubscribedSubscribers(string resource)
        {
            var empty = new List<string>();
            if (resource.IsNullOrEmpty()) return empty;

            return GetSubscriptionsByResource(resource).Select(s => s.Subscriber).ToList();
        }

        public bool HasSubscriber(string resource)
        {
            return _resources.ContainsKey(resource);
        }

        public int CountOfSubscriptions(string resource)
        {
            if (!_resources.ContainsKey(resource)) return 0;
            return _resources[resource].Count;
        }

        public bool Exists(string subscriber, string resource)
        {
            if (subscriber.IsNullOrEmpty()) return false;
            if (resource.IsNullOrEmpty()) return false;

            return Exists(subscriber, resource, string.Empty);
        }

        public bool Exists(string subscriber, string resource, string owner)
        {
            if (subscriber.IsNullOrEmpty()) return false;
            if (resource.IsNullOrEmpty()) return false;

            return Exists(new Subscription(subscriber, resource, owner));
        }

        public bool Exists(Subscription subscription)
        {
            if (subscription == null) return false;

            if (_resources.ContainsKey(subscription.Resource))
            {
                return _resources[subscription.Resource].ContainsKey(subscription.GetHashCode());
            }
            return false;
        }
    }
}
