using System.Reflection;
using Autofac;
using Autofac.Core;
using Autofac.Core.Resolving;
using Autofac.Core.Resolving.Pipeline;

namespace AutofacExtensions;

public static class AutofacReplaceTypeWithGenericTypeExtensions
{
    private static readonly FieldInfo segmentedStackArrayField =
        typeof( SegmentedStack<ResolveRequestContext> )
            .GetField( "_array", BindingFlags.NonPublic|BindingFlags.Instance );

    public static ContainerBuilder ReplaceTypeWithGenericTypeBasedOnRequestingType<T, TGeneric>(
        this ContainerBuilder builder )
        where TGeneric : T
    {
        builder.RegisterServiceMiddleware<T>(
            PipelinePhase.ResolveRequestStart,
            ( context, next ) =>
            {
                if( context.Operation.RequestDepth == 1 )
                {
                    next( context );
                }
                else
                {
                    IComponentRegistration registration;

                    if( context.Operation.RequestDepth == 2 )
                    {
                        registration = context.Operation.InitiatingRequest?.Registration;
                    }
                    else
                    {
                        var progressRequest = context.Operation.InProgressRequests.FirstOrDefault();

                        if( progressRequest == null )
                        {
                            // context.Operation.InProgressRequests is Enumerable backed by
                            // SegmentedStack<ResolveRequestContext>. SegmentedStack<> pretends to be empty when
                            // Decorators are resolved.
                            // SegmentedStack<> keeps its values in an array.

                            var stack = (ResolveRequestContext[])
                                segmentedStackArrayField.GetValue( context.Operation.InProgressRequests );

                            var i = 1;

                            while( i < stack!.Length && stack[i] != null )
                            {
                                ++i;
                            }

                            progressRequest = stack[i - 1];
                        }

                        registration = progressRequest.Registration;
                    }

                    var initiatingType = registration?.Activator.LimitType;

                    context.Instance = context.Resolve( typeof( TGeneric )
                        .GetGenericTypeDefinition()
                        .MakeGenericType( initiatingType! ) );
                }
            } );

        return builder;
    }
}