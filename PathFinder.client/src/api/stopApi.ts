import apiService from "@/api/apiService.ts";

export interface Stop {
    id: string;
    feedId: string;
    name: string;
    latitude: number;
    longitude: number;
    zoneId: string;
}

export const getStopsByFeedId = async (feedId: string): Promise<Stop[]> => {
    const response = await apiService.get<Stop[]>(`/Stop/${feedId}`);
    return response.data;
}