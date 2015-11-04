import React from 'react';
import Relay from 'react-relay';
import StarWarsCharacter from './StarWarsCharacter';

import StarWarsShip from './StarWarsShip';
import StarWarsShipForm from '../containers/StarWarsShipForm';
import RaisedButton from 'material-ui/lib/raised-button';

export default class Application extends React.Component {
    render() {
        var {humans, factions} = this.props.viewer;
      
        return (
        <div>
            <h1>StarWars Humans</h1>
            <ol>
                {humans.map(human => (
                <li key={human.id}>
                    <h2>{human.name}</h2>
                    { human.friends.length > 0 ? <p>His friends</p> : '' }
                    <ul>
                    {human.friends.map(c => (
                        <li key={c.id}><StarWarsCharacter character={c} /></li>
                    ))}
                    </ul>
                </li>
                ))}
            </ol>
            <h1>Factions</h1>
            <ol>
            {factions.map( faction =>(
                <li key={faction.factionId}>
                    <h2>{faction.factionName}</h2>
                    { faction.ships.length > 0 ? <p> Ships </p>: '' }
                    <ul>
                        { faction.ships.map( ship => (
                            <li key={ship.id}><StarWarsShip ship={ship} /></li>
                        ))}
                        <li>
                            <StarWarsShipForm faction={faction} />
                        </li>
                    </ul>
                </li>
            ))}
            </ol>
      </div>
    );
   }
}