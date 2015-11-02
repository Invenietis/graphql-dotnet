/// <reference path="react.d.ts" />

import Relay = __Relay;

declare namespace __Relay {
    class Route {
        constructor(initialParams: any);
    }

    class QL {

    }

    interface Container{
        fragments?: any;
        initialVariables?: any;
        prepareVariables?: any;
    }

    interface RootContainer extends React.ClassicComponentClass<any> {
        Component: Container;
        route: Route;
        forceFetch: boolean;
        renderLoading: React.ReactElement<any>;
        renderFetched(
            data: { [propName: string]: any },
            readyState: { stale: boolean }
        ): React.ReactElement<any>
        setVariables(partialVariables?: any, onReadyStateChange?: Function): void;
        getPendingTransactions(record: any): Array<any>;
    }


    type RelayRootContainer = React.ReactElement<RootContainer>;

    function createContainer<P,S>(component: React.Component<P, S>, containerDefinition: Container)
}

declare module "react-relay" {
    export = __Relay;
}

declare namespace JSX {
    import Relay = __Relay;
    //interface Element extends Relay..ReactElement<any> { }

    interface IntrinsicElements {
        RootContainer: Relay.RootContainer
    }
}