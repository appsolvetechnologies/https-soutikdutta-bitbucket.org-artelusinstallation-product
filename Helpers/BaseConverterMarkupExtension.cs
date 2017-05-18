
namespace Helpers
{
    using System;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// Initializes a new instance of the BaseConverterMarkupExtension class
    /// </summary>
    /// <typeparam name="T">The type of the converter</typeparam>
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class BaseConverterMarkupExtension<T> : MarkupExtension
        where T : class, IValueConverter, new()
    {
        /// <summary>
        /// The static type converter
        /// </summary>
        private static T converter = new T();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                return converter;
            }
        }

        /// <summary>
        /// Provides an object value for the serviceProvider
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns>The objct value</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter;
        }
    }
}
