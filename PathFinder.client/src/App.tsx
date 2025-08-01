import React, {useCallback, useEffect, useState} from 'react'
import {MapContainer, Marker, Popup, TileLayer} from 'react-leaflet'
import {Autocomplete, Box, CircularProgress, FormControlLabel, Stack, Switch, TextField} from "@mui/material";
import {getStopsByFeedId, Stop} from "@/api/stopApi.ts";
import {GtfsFeedInfo, quickSearch, advancedSearch} from "@/api/searchApi.ts";

const App: React.FC = () => {
    const [stops, setStops] = useState<Stop[]>([]);
    const [searchResults, setSearchResults] = useState<GtfsFeedInfo[]>([]);
    const [selectedFeed, setSelectedFeed] = useState<GtfsFeedInfo | null>(null);
    const [loading, setLoading] = useState(false);
    const [searchLoading, setSearchLoading] = useState(false);
    const [searchQuery, setSearchQuery] = useState('');
    const [isAdvancedSearch, setIsAdvancedSearch] = useState(false);
    const [inputValue, setInputValue] = useState('');
    const [timeoutId, setTimeoutId] = useState<NodeJS.Timeout | null>(null);

    const debounceSearch = useCallback((query: string) => {
        if (timeoutId) {
            clearTimeout(timeoutId);
        }

        if (query.length < 3) {
            setSearchResults([]);
            return;
        }

        const newTimeoutId = setTimeout(async () => {
            setSearchLoading(true);
            try {
                const results = isAdvancedSearch ? await advancedSearch(query) : await quickSearch(query);
                setSearchResults(results);
            } catch (error) {
                console.error('Search error:', error);
                setSearchResults([]);
            } finally {
                setSearchLoading(false);
            }
        }, 500);

        setTimeoutId(newTimeoutId);
    }, [isAdvancedSearch]);

    useEffect(() => {
        return () => {
            if (timeoutId) {
                clearTimeout(timeoutId);
            }
        };
    }, [timeoutId]);

    useEffect(() => {
        if (selectedFeed) {
            setLoading(true);
            getStopsByFeedId(selectedFeed.id)
                .then(setStops)
                .catch(console.error)
                .finally(() => setLoading(false));
        }
    }, [selectedFeed]);

    useEffect(() => {
        debounceSearch(searchQuery);
    }, [searchQuery, debounceSearch]);

    return (
        <Box sx={{ height: '100vh', width: '100%', display: 'flex', flexDirection: 'column' }}>
            <Stack direction="row" spacing={2} sx={{ p: 2 }} alignItems="center">
                <Autocomplete
                    options={searchResults}
                    getOptionLabel={(option) => `${option.provider} (${option.id})`}
                    value={selectedFeed}
                    onChange={(_, newValue) => setSelectedFeed(newValue)}
                    onInputChange={(_, newInputValue) => {
                        setInputValue(newInputValue);
                        setSearchQuery(newInputValue);
                    }}
                    inputValue={inputValue}
                    loading={searchLoading}
                    renderInput={(params) => (
                        <TextField
                            {...params}
                            label="Search GTFS Feeds"
                            variant="outlined"
                            placeholder={isAdvancedSearch ? "Advanced search..." : "Quick search (min 3 chars)..."}
                        />
                    )}
                    sx={{ width: 400 }}
                    noOptionsText={
                        searchQuery.length < 3
                            ? "Type at least 3 characters"
                            : "No results found"
                    }
                />

                <FormControlLabel
                    control={
                        <Switch checked={isAdvancedSearch}
                            onChange={(e) => setIsAdvancedSearch(e.target.checked)}
                            color="primary"
                        />
                    }
                    label="Advanced Search"
                />

                {loading ?? <CircularProgress size={24} />}
            </Stack>

            <MapContainer
                center={[51.505, -0.09]}
                zoom={13}
                style={{ height: '100%', width: '100%' }}
            >
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                {stops.map((stop) => (
                    <Marker
                        key={stop.id}
                        position={[stop.latitude, stop.longitude]}
                    >
                        <Popup>
                            <div>
                                <h3>{stop.name}</h3>
                                <p>Zone: {stop.zoneId}</p>
                                <p>ID: {stop.id}</p>
                            </div>
                        </Popup>
                    </Marker>
                ))}
            </MapContainer>
        </Box>
    );
};

export default App;
