import React from 'react';
import Relay from 'react-relay';
import RaisedButton from 'material-ui/lib/raised-button';
import IntroduceShipMutation from '../mutations/IntroduceShipMutation';

export default class StarWarsShipForm extends React.Component {
    handleClick = () => {
        // To perform a mutation, pass an instance of one to `Relay.Store.update`
        let mutation = new IntroduceShipMutation({
            faction: this.props.faction,
            ship: { shipName: this.state.shipName }
         });
        Relay.Store.update( mutation );
    }
    state = {shipName: 'Enter ship name'}
    handleChange = (event) => {
        this.setState({shipName: event.target.value});
    }
    render() {
        var {faction} = this.props;
        return (
            <form>
                <h2>Introduce a ship for {faction.factionName}</h2>
                <p>
                    <label htmlFor="tbShipName">Enter ship name </label>
                    <input name="tbShipName" type="text" value={this.state.shipName} onChange={this.handleChange} />
                </p>
                <RaisedButton onClick={this.handleClick} > Introduce Ship </RaisedButton>
            </form>
        );
    }
}