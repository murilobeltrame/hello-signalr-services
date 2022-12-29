import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import axios from "axios"

const AUTH_URL = process.env.REACT_APP_AUTH_URL!
const HUB_URL = process.env.REACT_APP_HUB_URL!

class Connector {
    private connection: HubConnection
    public events: (onMessageReceived: (message: string) => void) => void
    static instance: Connector

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl(HUB_URL, {
                accessTokenFactory: async () => {
                    const result = await axios.post(AUTH_URL, {name: 'theuser'})
                    return result.data
                }
            })
            .withAutomaticReconnect()
            .configureLogging(LogLevel.Trace)
            .build()
        this.connection.start().catch(error => console.error(error))
        this.events = (onMessageReceived) => {
            this.connection.on('messaging', (message) => {
                console.log(`Received ${message}`)
                onMessageReceived(message)
            })
        }
    }

    public newMessage = (message: string) => {
        console.log(`Sending ${message}`)
        this.connection
            .send('newMessage', message)
            .then(_ => console.log('Message sent'))
    }

    public static getInstance(): Connector {
        if (!Connector.instance) Connector.instance = new Connector()
        return Connector.instance
    }
}

export default Connector.getInstance