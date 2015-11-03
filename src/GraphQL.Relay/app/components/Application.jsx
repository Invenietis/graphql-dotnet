/**
 * This file provided by Facebook is for non-commercial testing and evaluation
 * purposes only.  Facebook reserves all rights not expressly granted.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * FACEBOOK BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN
 * ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
 
import React from 'react';
import Relay from 'react-relay';
import StarWarsCharacter from './StarWarsCharacter';
import RaisedButton from 'material-ui/lib/raised-button';

export default class Application extends React.Component {
    addFriend (c) {
        c.friends.push({
            name: 'lol',
            id: new Date().getTime()
        });
        
        this.setState({});
    }
    
    get initialState(){
        return {
            humans: []
        };
    }
    render() {
        var {humans} = this.props.viewer;
      
        return (
        <div>
            <h1>StarWars Humans</h1>
            <ol>
                {humans.map(human => (
                <li key={human.id}>
                    <h1>{human.name}</h1>
                    { human.friends.length > 0 ? <p>His friends</p> : '' }
                    <ol>
                    {human.friends.map(c => (
                        <li key={c.id}><StarWarsCharacter character={c} /></li>
                    ))}
                    </ol>
                    <RaisedButton onClick={() => this.addFriend(human)} > Add friend </RaisedButton>
                </li>
                ))}
            </ol>
      </div>
    );
   }
}