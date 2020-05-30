using System;
using System.Linq.Expressions;

namespace EUSignNetProject.Extensions
{
    public static class GuardExtensions
    {
        /// <summary>
        /// Throws exception with the parameter name if the value of nullable parameter is null.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown the exception with the parameter name if the value is null.</exception>
        /// <typeparam name="TSource">The type.</typeparam>
        /// <param name="value">The nullable type to guard.</param>
        /// <param name="lambda">The expression to get the parameter name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting is needed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Generic type is used to scope the function.")]
        public static void Guard<TSource>(this TSource? value, Expression<Func<TSource?>> lambda)
             where TSource : struct
        {
            if (!value.HasValue && lambda != null)
            {
                var member = lambda.Body as MemberExpression;
                var name = member != null ? member.Member.Name : "N/A";

                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws exception with the parameter name if the value of reference type parameter is null.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown the exception with the parameter name if the value is null.</exception>
        /// <typeparam name="TSource">The type.</typeparam>
        /// <param name="value">The reference type to guard.</param>
        /// <param name="lambda">The expression to get the parameter name.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting is needed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Generic type is used to scope the function.")]
        public static void Guard<TSource>(this TSource value, Expression<Func<TSource>> lambda)
            where TSource : class
        {
            if (lambda != null && (value == default(TSource) || (typeof(TSource) == typeof(string) && string.IsNullOrWhiteSpace(value as string))))
            {
                var member = lambda.Body as MemberExpression;
                var name = member != null ? member.Member.Name : "N/A";
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws exception with the parameter name if the condition is true.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown the exception with the parameter name if the value is null.</exception>
        /// <typeparam name="TSource">The type.</typeparam>
        /// <param name="value">The reference type to guard.</param>
        /// <param name="lambda">The expression to get the parameter name.</param>
        /// <param name="throwException">If true an exception is thrown.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value", Justification = "Parameter is used for qualifying as extension method")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nesting is needed")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "Generic type is used to scope the function.")]
        public static void Guard<TSource>(this TSource value, Expression<Func<TSource>> lambda, bool throwException)
        {
            if (lambda != null && throwException)
            {
                var member = lambda.Body as MemberExpression;
                var name = member != null ? member.Member.Name : "N/A";
                throw new ArgumentOutOfRangeException(name);
            }
        }
    }
}
