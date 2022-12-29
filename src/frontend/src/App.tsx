import React, { useEffect, useState } from 'react';
import './App.css';
import Connector from './signalr-connection'

function App() {

  const {newMessage, events} = Connector()
  const [message, setMessage] = useState('InitialValue')

  useEffect(() => events((message) => setMessage(message)))

  return (
    <div className="App">
      <span>Message from SignalR </span>
      <span style={{color: 'green', fontWeight: 'bold'}}>{message}</span>
      <br />
      <button onClick={() => newMessage('Hello from React')}>Send hello message</button>
    </div>
  );
}

export default App;
