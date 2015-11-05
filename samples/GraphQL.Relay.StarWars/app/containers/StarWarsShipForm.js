
import Relay from 'react-relay';
import StarWarsShipForm from '../components/StarWarsShipForm';
import IntroduceShipMutation from '../mutations/IntroduceShipMutation';

export default Relay.createContainer(StarWarsShipForm, {
  fragments: {
    // You can compose a mutation's query fragments like you would those
    // of any other RelayContainer. This ensures that the data depended
    // upon by the mutation will be fetched and ready for use.
    faction: () => Relay.QL`
      fragment on Faction {
        ships
        ${IntroduceShipMutation.getFragment('faction')}
      }
    `,
  },
});