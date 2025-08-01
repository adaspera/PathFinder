import apiService from "@/api/apiService.ts";

export interface GtfsFeedInfo {
    id: string;
    provider: string;
}

export const quickSearch = async (query: string): Promise<GtfsFeedInfo[]> => {
    const response =
        await apiService.get<GtfsFeedInfo[]>(`/citysearch?query=${query}`);
    return response.data;
}

export const advancedSearch = async (query: string): Promise<GtfsFeedInfo[]> => {
    const response =
        await apiService.get<GtfsFeedInfo[]>(`/citysearch/advanced?query=${query}`);
    return response.data;
}