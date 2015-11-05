import Relay from 'react-relay';

export default class IntroduceShipMutation extends Relay.Mutation {
  
  // This mutation declares a dependency on the faction
  // into which this ship is to be introduced.
  static fragments = {
    faction: () => Relay.QL`
      fragment on Faction {
        factionId,
      }
    `
  };
  // viewer: () => Relay.QL`
  //  fragment on Query {
  //      factions {
  //           factionId,
  //           factionName,
  //           ships {
  //               id,
  //               shipName
  //           }
  //       }
  //  }`
   
  // This method should return a GraphQL operation that represents
  // the mutation to be performed. This presumes that the server
  // implements a mutation type named introduceShip.
  getMutation() {
    return Relay.QL`mutation { introduceShip }`;
  }
  // Use this method to prepare the variables that will be used as
  // input to the mutation. Our ‘likeStory’ mutation takes exactly
  // one variable as input – the ID of the story to like.
  getVariables() {
    return {
      factionId: this.props.faction.factionId,
      shipName: this.props.ship.shipName
    };
  }
  // Use this method to design a ‘fat query’ – one that represents every
  // field in your data model that could change as a result of this mutation.
  // Introducing a ship will add it to a faction's fleet, so we
  // specify the faction's ships connection as part of the fat query.
  getFatQuery() {
    return Relay.QL`
      fragment on IntroduceShipPayload {
        faction { 
          ships {
            id
            shipName
          } 
        },
        newShipEdge {
          id 
          shipName
        }
      }
    `;
  }
  
  getConfigs() {
    return [{
      type: 'RANGE_ADD',
      parentName: 'faction',
      parentID: this.props.faction.factionId,
      connectionName: 'ships',
      edgeName: 'newShipEdge',
      rangeBehaviors: {
        // When the ships connection is not under the influence
        // of any call, append the ship to the end of the connection
        '': 'append',
        // Prepend the ship, wherever the connection is sorted by age
        'orderby(newest)': 'prepend',
      },
    }];
  }
  
}