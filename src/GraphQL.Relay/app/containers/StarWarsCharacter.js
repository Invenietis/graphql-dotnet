import StarWarsCharacter from '../components/StarWarsCharacter';
import Relay from 'react-relay';

export default Relay.createContainer(StarWarsCharacter, {
    fragments: {
        character: () => Relay.QL`
      fragment on Character {
        id
        name
      }
    `,
    },
});