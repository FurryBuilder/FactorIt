using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using FactorIt.Collections.Wrappers;
using FactorIt.Extensions;

namespace FactorIt.Collections
{
	/// <summary>
	/// Implementation of an observable dictionary that can safely be used outside
	/// of the Dispatcher thread. This class wraps a standard Dictionary of TKey
	/// and TValue to provide notifications when its content changes via the
	/// CollectionChanged event.
	/// </summary>
	/// <typeparam name="TKey">The type of the key in the dictionary</typeparam>
	/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
	internal class ObservableDictionary<TKey, TValue> :
		IDictionary<TKey, TValue>,
		ICollection<KeyValuePair<TKey, TValue>>,
		IDictionary,
		ICollection,
		IReadOnlyDictionary<TKey, TValue>,
		IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
		IEnumerable<KeyValuePair<TKey, TValue>>,
		IEnumerable,
		INotifyCollectionChanged
	{
		public static class Constants
		{
			public const int DefaultNotificationIndex = -1;

			public const string WrongKeyType = "Expected the provided key to be of type {0} but received type {1}";
			public const string WrongValueType = "Expected the provided value to be of type {0} but received type {1}";
		}

		private readonly Dictionary<TKey, TValue> _dictionary;

		#region Constructors

		public ObservableDictionary()
		{
			_dictionary = new Dictionary<TKey, TValue>();
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			_dictionary = new Dictionary<TKey, TValue>(dictionary);
		}

		public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
		}

		public ObservableDictionary(IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(comparer);
		}

		public ObservableDictionary(int capacity)
		{
			_dictionary = new Dictionary<TKey, TValue>(capacity);
		}

		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
		{
			_dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
		}

		#endregion

		#region INotifyCollectionChanged Members

		private void NotifyCollectionAdd(object item, int index)
		{
			NotifyCollectionChanged(new AddCollectionChangedEventArgs(item, index));
		}

		private void NotifyCollectionRemove(object item, int oldIndex)
		{
			NotifyCollectionChanged(new RemoveCollectionChangedEventArgs(item, oldIndex));
		}

		private void NotifyCollectionReplace(object newItem, object oldItem, int index)
		{
			NotifyCollectionChanged(new ReplaceCollectionChangedEventArgs(newItem, oldItem, index));
		}

		private void NotifyCollectionReset()
		{
			NotifyCollectionChanged(new ResetCollectionChangedEventArgs());
		}

		private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			CollectionChanged.Maybe(h => h.Invoke(this, args));
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion

		#region IEnumerable Members

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion

		#region IReadOnlyDictionary<TKey, TValue> Members

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
		{
			get { return _dictionary.Values; }
		}

		TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key]
		{
			get { return _dictionary[key]; }
		}

		#endregion

		#region ICollection Members

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		void ICollection.CopyTo(Array array, int index)
		{
			((ICollection)_dictionary).CopyTo(array, index);
		}

		int ICollection.Count
		{
			get { return ((ICollection)_dictionary).Count; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool ICollection.IsSynchronized
		{
			get { return ((ICollection)_dictionary).IsSynchronized; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		object ICollection.SyncRoot
		{
			get { return ((ICollection)_dictionary).SyncRoot; }
		}

		#endregion

		#region IDictionary Members

		void IDictionary.Add(object key, object value)
		{
			((IDictionary)_dictionary).Add(key, value);

			NotifyCollectionAdd(new KeyValuePair<TKey, TValue>((TKey)key, (TValue)value), Constants.DefaultNotificationIndex);
		}

		public void Clear()
		{
			((IDictionary)_dictionary).Clear();

			NotifyCollectionReset();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool IDictionary.Contains(object key)
		{
			return ((IDictionary)_dictionary).Contains(key);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IDictionary)_dictionary).GetEnumerator();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool IDictionary.IsFixedSize
		{
			get { return ((IDictionary)_dictionary).IsFixedSize; }
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool IDictionary.IsReadOnly
		{
			get { return ((IDictionary)_dictionary).IsReadOnly; }
		}

		ICollection IDictionary.Keys
		{
			get { return ((IDictionary)_dictionary).Keys; }
		}

		void IDictionary.Remove(object key)
		{
			if (!(key is TKey))
			{
				return;
			}

			TValue value;

			if (!_dictionary.TryGetValue((TKey)key, out value))
			{
				return;
			}

			((IDictionary)_dictionary).Remove(key);

			NotifyCollectionRemove(new KeyValuePair<TKey, TValue>((TKey)key, value), Constants.DefaultNotificationIndex);
		}

		ICollection IDictionary.Values
		{
			get { return ((IDictionary)_dictionary).Values; }
		}

		object IDictionary.this[object key]
		{
			get
			{
				return ((IDictionary)_dictionary)[key];
			}
			set
			{
				if (!(key is TKey))
				{
					throw new ArgumentException(string.Format(Constants.WrongKeyType, typeof(TKey), key.GetType()));
				}

				if (!(value is TValue))
				{
					throw new ArgumentException(string.Format(Constants.WrongValueType, typeof(TKey), key.GetType()));
				}

				if (((IDictionary)_dictionary).Contains(new KeyValuePair<TKey, TValue>((TKey)key, (TValue)value)))
				{
					var oldValue = ((IDictionary)_dictionary)[key];

					((IDictionary)_dictionary)[key] = value;

					NotifyCollectionReplace(value, oldValue, Constants.DefaultNotificationIndex);
				}
				else
				{
					((IDictionary)_dictionary)[key] = value;

					NotifyCollectionAdd(value, Constants.DefaultNotificationIndex);
				}
			}
		}

		#endregion

		#region ICollection<KeyValuePair<TKey, TValue>> Members

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Add(item);

			NotifyCollectionAdd(item, Constants.DefaultNotificationIndex);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Clear();

			NotifyCollectionReset();
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Contains(item);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Hidden to replicate the behavior of Dictionary<TKey, TValue>")]
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).IsReadOnly; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			if (!((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).Remove(item))
			{
				return false;
			}

			NotifyCollectionRemove(item, Constants.DefaultNotificationIndex);

			return true;
		}

		#endregion

		#region IDictionary<TKey, TValue> Members

		public void Add(TKey key, TValue value)
		{
			_dictionary.Add(key, value);

			NotifyCollectionAdd(new KeyValuePair<TKey, TValue>(key, value), Constants.DefaultNotificationIndex);
		}

		public ICollection<TKey> Keys
		{
			get { return _dictionary.Keys; }
		}

		public bool Remove(TKey key)
		{
			TValue value;

			if (!_dictionary.TryGetValue(key, out value))
			{
				return false;
			}


			if (!_dictionary.Remove(key))
			{
				return false;
			}

			NotifyCollectionRemove(new KeyValuePair<TKey, TValue>(key, value), Constants.DefaultNotificationIndex);

			return true;
		}

		public ICollection<TValue> Values
		{
			get { return _dictionary.Values; }
		}

		public TValue this[TKey key]
		{
			get
			{
				return _dictionary[key];
			}
			set
			{
				if (_dictionary.ContainsKey(key))
				{
					var oldValue = _dictionary[key];

					_dictionary[key] = value;

					NotifyCollectionReplace(value, oldValue, Constants.DefaultNotificationIndex);
				}
				else
				{
					_dictionary[key] = value;

					NotifyCollectionAdd(value, Constants.DefaultNotificationIndex);
				}
			}
		}

		#endregion

		#region Dictionary<TKey, TValue> Members

		public IEqualityComparer<TKey> Comparer
		{
			get { return _dictionary.Comparer; }
		}

		public int Count
		{
			get { return _dictionary.Count; }
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		#endregion
	}
}