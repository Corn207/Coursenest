import { useParams } from "react-router";
import axios from "axios";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";
import config from "~/config";

export default function Material() {
    const { materialId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [material, setMaterial] = useState({});
    const tokenStr = localStorage.getItem('accessToken');

    useEffect(() => {
        setIsLoading(true);
        axios.get(`${config.baseUrl}/api/units/${materialId}/material`, {
            headers: { Authorization: `Bearer ${tokenStr}` }
        })
        .then((res) => {
            setMaterial(res.data)
        })
        .finally(() => setIsLoading(false))
    }, [materialId]);

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{material.title}</h3>
            <h5 style={{color: "red", marginBottom: 20}}>{material.requiredMinutes} minutes</h5>
            <p>{material.content}</p>
        </div>
    )
}