import Application from '../components/Application';
import Relay from 'react-relay';

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
            }
        }
        `
    } 
});