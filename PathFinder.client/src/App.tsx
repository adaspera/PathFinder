import { useEffect, useState } from 'react'
import apiClient from './api/apiService';


const App: React.FC = () => {
    const [data, setData] = useState<string[]>([]);

    useEffect(() => {
        apiClient.get('/test') // Replace with your backend endpoint
            .then(response => setData(response.data))
            .catch(error => console.error(error));
    }, []);

    return (
        <div>
            <h1>Data from Backend</h1>
            <ul>
                {data.map((item, index) => (
                    <li key={index}>{item}</li>
                ))}
            </ul>
        </div>
    );
};

export default App
