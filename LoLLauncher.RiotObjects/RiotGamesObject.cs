using LoLLauncher.RiotObjects.Platform.Game;
using LoLLauncher.RiotObjects.Platform.Reroll.Pojo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LoLLauncher.RiotObjects
{
	public abstract class RiotGamesObject
	{
		public virtual string TypeName
		{
			get;
			private set;
		}

		[InternalName("futureData")]
		public int FutureData
		{
			get;
			set;
		}

		[InternalName("dataVersion")]
		public int DataVersion
		{
			get;
			set;
		}

		public TypedObject GetBaseTypedObject()
		{
			TypedObject typedObject = new TypedObject(this.TypeName);
			Type type = base.GetType();
			PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				InternalNameAttribute internalNameAttribute = propertyInfo.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
				if (internalNameAttribute != null)
				{
					object obj = null;
					Type propertyType = propertyInfo.PropertyType;
					string arg_5F_0 = propertyType.Name;
					if (propertyType == typeof(int[]))
					{
						int[] array = propertyInfo.GetValue(this) as int[];
						if (array != null)
						{
							obj = array.Cast<object>().ToArray<object>();
						}
					}
					else if (propertyType == typeof(double[]))
					{
						double[] array2 = propertyInfo.GetValue(this) as double[];
						if (array2 != null)
						{
							obj = array2.Cast<object>().ToArray<object>();
						}
					}
					else if (propertyType == typeof(string[]))
					{
						string[] array3 = propertyInfo.GetValue(this) as string[];
						if (array3 != null)
						{
							obj = array3.Cast<object>().ToArray<object>();
						}
					}
					else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
					{
						IList list = propertyInfo.GetValue(this) as IList;
						if (list != null)
						{
							object[] array4 = new object[list.Count];
							list.CopyTo(array4, 0);
							List<object> list2 = new List<object>();
							object[] array5 = array4;
							for (int j = 0; j < array5.Length; j++)
							{
								object obj2 = array5[j];
								Type type2 = obj2.GetType();
								if (typeof(RiotGamesObject).IsAssignableFrom(type2))
								{
									RiotGamesObject riotGamesObject = obj2 as RiotGamesObject;
									obj = riotGamesObject.GetBaseTypedObject();
								}
								else
								{
									obj = obj2;
								}
								list2.Add(obj);
							}
							obj = TypedObject.MakeArrayCollection(list2.ToArray());
						}
					}
					else if (typeof(RiotGamesObject).IsAssignableFrom(propertyType))
					{
						RiotGamesObject riotGamesObject2 = propertyInfo.GetValue(this) as RiotGamesObject;
						if (riotGamesObject2 != null)
						{
							obj = riotGamesObject2.GetBaseTypedObject();
						}
					}
					else
					{
						obj = propertyInfo.GetValue(this);
					}
					typedObject.Add(internalNameAttribute.Name, obj);
				}
			}
			Type baseType = type.BaseType;
			if (baseType != null)
			{
				PropertyInfo[] properties2 = baseType.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				for (int k = 0; k < properties2.Length; k++)
				{
					PropertyInfo propertyInfo2 = properties2[k];
					InternalNameAttribute internalNameAttribute2 = propertyInfo2.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
					if (internalNameAttribute2 != null && !typedObject.ContainsKey(internalNameAttribute2.Name))
					{
						typedObject.Add(internalNameAttribute2.Name, propertyInfo2.GetValue(this));
					}
				}
			}
			return typedObject;
		}

		public virtual void DoCallback(TypedObject result)
		{
		}

		public void SetFields<T>(T obj, TypedObject result)
		{
			if (result == null)
			{
				return;
			}
			this.TypeName = result.type;
			PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				InternalNameAttribute internalNameAttribute = propertyInfo.GetCustomAttributes(typeof(InternalNameAttribute), false).FirstOrDefault<object>() as InternalNameAttribute;
				if (internalNameAttribute != null)
				{
					Type propertyType = propertyInfo.PropertyType;
					object value;
					if (result.TryGetValue(internalNameAttribute.Name, out value))
					{
						try
						{
							if (result[internalNameAttribute.Name] == null)
							{
								value = null;
							}
							else if (propertyType == typeof(string))
							{
								value = Convert.ToString(result[internalNameAttribute.Name]);
							}
							else if (propertyType == typeof(int))
							{
								value = Convert.ToInt32(result[internalNameAttribute.Name]);
							}
							else if (propertyType == typeof(long))
							{
								value = Convert.ToInt64(result[internalNameAttribute.Name]);
							}
							else if (propertyType == typeof(double))
							{
								value = Convert.ToInt64(result[internalNameAttribute.Name]);
							}
							else if (propertyType == typeof(bool))
							{
								value = Convert.ToBoolean(result[internalNameAttribute.Name]);
							}
							else if (propertyType == typeof(DateTime))
							{
								value = result[internalNameAttribute.Name];
							}
							else if (propertyType == typeof(TypedObject))
							{
								value = (TypedObject)result[internalNameAttribute.Name];
							}
							else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(List<>))
							{
								object[] array = result.GetArray(internalNameAttribute.Name);
								Type type = propertyType.GetGenericArguments()[0];
								Type type2 = typeof(List<>).MakeGenericType(new Type[]
								{
									type
								});
								IList list = (IList)Activator.CreateInstance(type2);
								object[] array2 = array;
								for (int j = 0; j < array2.Length; j++)
								{
									object obj2 = array2[j];
									if (obj2 == null)
									{
										list.Add(null);
									}
									if (type == typeof(string))
									{
										list.Add((string)obj2);
									}
									else if (type == typeof(Participant))
									{
										TypedObject typedObject = (TypedObject)obj2;
										if (typedObject.type == "com.riotgames.platform.game.BotParticipant")
										{
											list.Add(new BotParticipant(typedObject));
										}
										else if (typedObject.type == "com.riotgames.platform.game.ObfruscatedParticipant")
										{
											list.Add(new ObfruscatedParticipant(typedObject));
										}
										else if (typedObject.type == "com.riotgames.platform.game.PlayerParticipant")
										{
											list.Add(new PlayerParticipant(typedObject));
										}
										else if (typedObject.type == "com.riotgames.platform.reroll.pojo.AramPlayerParticipant")
										{
											list.Add(new AramPlayerParticipant(typedObject));
										}
									}
									else if (type == typeof(int))
									{
										list.Add((int)obj2);
									}
									else
									{
										list.Add(Activator.CreateInstance(type, new object[]
										{
											obj2
										}));
									}
								}
								value = list;
							}
							else if (propertyType == typeof(Dictionary<string, object>))
							{
								Dictionary<string, object> dictionary = (Dictionary<string, object>)result[internalNameAttribute.Name];
								value = dictionary;
							}
							else if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Dictionary<, >))
							{
								Dictionary<string, object> dictionary2 = (Dictionary<string, object>)result[internalNameAttribute.Name];
								value = dictionary2;
							}
							else if (propertyType == typeof(int[]))
							{
								value = result.GetArray(internalNameAttribute.Name).Cast<int>().ToArray<int>();
							}
							else if (propertyType == typeof(string[]))
							{
								value = result.GetArray(internalNameAttribute.Name).Cast<string>().ToArray<string>();
							}
							else if (propertyType == typeof(object[]))
							{
								value = result.GetArray(internalNameAttribute.Name);
							}
							else if (propertyType == typeof(object))
							{
								value = result[internalNameAttribute.Name];
							}
							else
							{
								try
								{
									value = Activator.CreateInstance(propertyType, new object[]
									{
										result[internalNameAttribute.Name]
									});
								}
								catch (Exception innerException)
								{
									throw new NotSupportedException(string.Format("Type {0} not supported by flash serializer", propertyType.FullName), innerException);
								}
							}
							propertyInfo.SetValue(obj, value, null);
						}
						catch
						{
						}
					}
				}
			}
		}
	}
}
