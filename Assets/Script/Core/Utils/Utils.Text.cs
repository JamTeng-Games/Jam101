using System;
using System.Text;

namespace Jam.Core
{

    public static partial class Utils
    {
        public static class Text
        {
            private const int StringBuilderCapacity = 1024;

            [ThreadStatic] private static StringBuilder s_CachedStringBuilder = null;

            private static void CheckCachedStringBuilder()
            {
                if (s_CachedStringBuilder == null)
                {
                    s_CachedStringBuilder = new StringBuilder(StringBuilderCapacity);
                }
            }

            public static string Format<T>(string format, T arg)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2>(string format, T1 arg1, T2 arg2)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3, T4, T5, T6>(string format,
                                                                T1 arg1,
                                                                T2 arg2,
                                                                T3 arg3,
                                                                T4 arg4,
                                                                T5 arg5,
                                                                T6 arg6)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3, T4, T5, T6, T7>(string format,
                                                                    T1 arg1,
                                                                    T2 arg2,
                                                                    T3 arg3,
                                                                    T4 arg4,
                                                                    T5 arg5,
                                                                    T6 arg6,
                                                                    T7 arg7)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                return s_CachedStringBuilder.ToString();
            }

            public static string Format<T1, T2, T3, T4, T5, T6, T7, T8>(string format,
                                                                        T1 arg1,
                                                                        T2 arg2,
                                                                        T3 arg3,
                                                                        T4 arg4,
                                                                        T5 arg5,
                                                                        T6 arg6,
                                                                        T7 arg7,
                                                                        T8 arg8)
            {
                if (format == null)
                {
                    throw new Exception("Format is invalid.");
                }

                CheckCachedStringBuilder();
                s_CachedStringBuilder.Length = 0;
                s_CachedStringBuilder.AppendFormat(format, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                return s_CachedStringBuilder.ToString();
            }
        }
    }

}