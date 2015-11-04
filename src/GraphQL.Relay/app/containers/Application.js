import Relay from 'react-relay';
import Application from '../components/Application';
import IntroduceShipMutation from '../mutations/IntroduceShipMutation';

export default Relay.createContainer(Application, { 
    fragments: {
        viewer: () => Relay.QL`
            fragment on Query {
                humans {
                    id
                    name  
                    friends {
                        id
                        name
                    }
                },
                factions {
                    factionId,
                    factionName,
                    ships {
                        id,
                        shipName
                    }
                    ${IntroduceShipMutation.getFragment('faction')}
                }
            }`
    } 
    
});