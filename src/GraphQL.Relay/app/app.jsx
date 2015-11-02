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

import 'babel/polyfill';
import React from 'react';
import ReactDOM from 'react-dom';
import Relay from 'react-relay';
import Application from './containers/Application';
import ApplicationQueries from './routes/ApplicationQueries';

Relay.injectNetworkLayer(
  new Relay.DefaultNetworkLayer('/graphql', {
      headers: {
          Authorization: 'Basic SSdsbCBmaW5kIHNvbWV0aGluZyB0byBwdXQgaGVyZQ==',
      }
  })
);

ReactDOM.render(
  <Relay.RootContainer Component={Application} route={new ApplicationQueries()} />,
  document.getElementById('root')
);