#import <Foundation/Foundation.h>
#import <Metrix/Metrix-Swift.h>
#import "UnityInterface.h"

@interface MetrixPlugin: NSObject

- (void) initializeWithMetrixAppId:(NSString * _Nonnull)appId;
- (void) newEventWithSlug:(NSString * _Nonnull)slug;
- (void) newEventWithSlug:(NSString * _Nonnull)slug attributes:(NSDictionary<NSString *, NSString *> * _Nonnull)attributes;
- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue;
- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue currency:(NSInteger)currency;
- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue orderId:(NSString * _Nonnull)orderId;
- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue currency:(NSInteger)currency orderId:(NSString * _Nullable)orderId;
- (void) addUserAttributesWithUserAttrs:(NSDictionary<NSString *, NSString *> * _Nonnull)userAttrs;
- (void) setPushTokenWithPushToken:(NSString * _Nonnull)token;
- (void) setStoreWithStoreName:(NSString * _Nonnull)store;
- (void) setAppSecretWithSecretId:(NSInteger)secretId info1:(NSInteger)info1 info2:(NSInteger)info2 info3:(NSInteger)info3 info4:(NSInteger)info4;
- (void) setDefaultTrackerWithTrackerToken:(NSString * _Nonnull)trackerToken;
- (void) setUserIdListener;
- (void) setSessionIdListener;
- (void) setSessionNumberListener;
- (void) setOnDeeplinkResponseListener:(bool)shouldLaunchDeferredDeeplink;
- (void) setOnAttributionChangedListener;

@end

@implementation MetrixPlugin

static NSString *MANAGER_OBJECT = @"MetrixManager";
static NSString *ON_DEFERRED_DEEPLINK = @"OnDeferredDeeplink";
static NSString *ON_RECEIVE_USER_ID_LISTENER = @"OnReceiveUserIdListener";
static NSString *ON_ATTRIBUTION_CHANGE_LISTENER = @"OnAttributionChangeListener";
static NSString *SESSION_ID_CHANGE_LISTENER = @"SessionIDChangeListener";
static NSString *SESSION_NUMBER_CHANGE_LISTENER = @"SessionNumberChangeListener";

+ (MetrixPlugin*) sharedInstance {
    static MetrixPlugin * instance = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [[MetrixPlugin alloc] init];
    });
    return instance;
}

- (void) initializeWithMetrixAppId:(NSString * _Nonnull)appId
{
    [MetrixClient initializeWithMetrixAppId:appId];
}

- (void) setSessionNumberListener
{
    [MetrixClient setSessionNumberListener:^(NSInteger sessionNumber) {
        UnitySendMessage([MANAGER_OBJECT UTF8String], [SESSION_NUMBER_CHANGE_LISTENER UTF8String], [[NSString stringWithFormat:@"%ld", (long)sessionNumber] UTF8String]);
    }];
}

- (void) setSessionIdListener
{
    [MetrixClient setSessionIdListener:^(NSString *sessionId) {
        UnitySendMessage([MANAGER_OBJECT UTF8String], [SESSION_ID_CHANGE_LISTENER UTF8String], [sessionId UTF8String]);
    }];
}

- (void) newEventWithSlug:(NSString * _Nonnull)slug
{
    [MetrixClient newEventWithSlug:slug];
}

- (void) newEventWithSlug:(NSString * _Nonnull)slug attributes:(NSDictionary<NSString *, NSString *> * _Nonnull)attributes
{
    [MetrixClient newEventWithSlug:slug attributes:attributes];
}

- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue
{
    [MetrixClient newRevenueWithSlug:slug revenue:revenue];
}

- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue currency:(NSInteger)currency
{
    [MetrixClient newRevenueWithSlug:slug revenue:revenue currency:(RevenueCurrency)currency];
}

- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue orderId:(NSString * _Nonnull)orderId
{
    [MetrixClient newRevenueWithSlug:slug revenue:revenue orderId:orderId];
}

- (void) newRevenueWithSlug:(NSString * _Nonnull)slug revenue:(double)revenue currency:(NSInteger)currency orderId:(NSString * _Nullable)orderId
{
    [MetrixClient newRevenueWithSlug:slug revenue:revenue currency:(RevenueCurrency)currency orderId:orderId];
}

- (void) addUserAttributesWithUserAttrs:(NSDictionary<NSString *, NSString *> * _Nonnull)userAttrs
{
    [MetrixClient addUserAttributesWithUserAttrs:userAttrs];
}

- (void) setPushTokenWithPushToken:(NSString * _Nonnull)token
{
    [MetrixClient setPushTokenWithPushToken:token];
}

- (void) setStoreWithStoreName:(NSString * _Nonnull)store
{
    [MetrixClient setStoreWithStoreName:store];
}

- (void) setAppSecretWithSecretId:(NSInteger)secretId info1:(NSInteger)info1 info2:(NSInteger)info2 info3:(NSInteger)info3 info4:(NSInteger)info4
{
    [MetrixClient setAppSecretWithSecretId:secretId info1:info1 info2:info2 info3:info3 info4:info4];
}

- (void) setDefaultTrackerWithTrackerToken:(NSString * _Nonnull)trackerToken
{
    [MetrixClient setDefaultTrackerWithTrackerToken:trackerToken];
}

- (void) setUserIdListener
{
    [MetrixClient setUserIdListener:^(NSString *string) {
        UnitySendMessage([MANAGER_OBJECT UTF8String], [ON_RECEIVE_USER_ID_LISTENER UTF8String], [string UTF8String]);
    }];
}

- (void) setOnDeeplinkResponseListener:(bool)shouldLaunchDeferredDeeplink
{
    [MetrixClient setOnDeeplinkResponseListener:^BOOL(NSString *string) {
        UnitySendMessage([MANAGER_OBJECT UTF8String], [ON_DEFERRED_DEEPLINK UTF8String], [string UTF8String]);
        return shouldLaunchDeferredDeeplink;
    }];
}

- (void) setOnAttributionChangedListener
{
    [MetrixClient setOnAttributionChangedListener:^(AttributionData *data) {
        UnitySendMessage([MANAGER_OBJECT UTF8String], [ON_ATTRIBUTION_CHANGE_LISTENER UTF8String], [data.jsonValue UTF8String]);
    }];
}

@end

char* convertNSStringToCString(const NSString* nsString)
{
    if (nsString == NULL)
        return NULL;

    const char* nsStringUtf8 = [nsString UTF8String];
    //create a null terminated C string on the heap so that our string's memory isn't wiped out right after method's return
    char* cString = (char*)malloc(strlen(nsStringUtf8) + 1);
    strcpy(cString, nsStringUtf8);

    return cString;
}

NSMutableDictionary * convertNSStringToCDictionary(const NSString* parameters)
{
    NSArray *arr = [parameters componentsSeparatedByString: @"\n"];
    NSMutableDictionary *pdict = [NSMutableDictionary dictionary];
    for(int i=0; i < [arr count]; i++)
    {
        NSString *str1 = [arr objectAtIndex:i];
        NSRange range = [str1 rangeOfString:@"="];
        if (range.location != NSNotFound)
        {
            NSString *key = [str1 substringToIndex:range.location];
            NSString *val = [str1 substringFromIndex:range.location+1];
            [pdict setObject:val forKey:key];
        }
    }
    return pdict;
}

extern "C" {
    void _Initialize(const char * appId)
    {
        [[MetrixPlugin sharedInstance] initializeWithMetrixAppId:[NSString stringWithUTF8String:appId]];
    }

    void _SetSessionNumberListener()
    {
        [[MetrixPlugin sharedInstance] setSessionNumberListener];
    }
        
    void _SetSessionIdListener()
    {
        [[MetrixPlugin sharedInstance] setSessionIdListener];
    }
    
    void _NewEvent(const char * slug)
    {
        [[MetrixPlugin sharedInstance] newEventWithSlug:[NSString stringWithUTF8String:slug]];
    }
    
    void _NewAttributedEvent(const char * slug, const char * customAttributesString)
    {
        [[MetrixPlugin sharedInstance] newEventWithSlug:[NSString stringWithUTF8String:slug] attributes:convertNSStringToCDictionary([NSString stringWithUTF8String:customAttributesString])];
    }

    void _NewRevenue(const char * slug, double revenue)
    {
        [[MetrixPlugin sharedInstance] newRevenueWithSlug:[NSString stringWithUTF8String:slug] revenue:revenue];
    }

    void _NewRevenueCurrency(const char * slug, double revenue, int currency)
    {
        [[MetrixPlugin sharedInstance] newRevenueWithSlug:[NSString stringWithUTF8String:slug] revenue:revenue currency:currency];
    }
    
    void _NewRevenueFull(const char * slug, double revenue, int currency, const char * orderId)
    {
        [[MetrixPlugin sharedInstance] newRevenueWithSlug:[NSString stringWithUTF8String:slug] revenue:revenue currency:currency orderId:[NSString stringWithUTF8String:orderId]];
    }
    
    void _NewRevenueOrderId(const char * slug, double revenue, const char * orderId)
    {
        [[MetrixPlugin sharedInstance] newRevenueWithSlug:[NSString stringWithUTF8String:slug] revenue:revenue orderId:[NSString stringWithUTF8String:orderId]];
    }
    
    void _AddUserAttributes(const char * userAttrs)
    {
        [[MetrixPlugin sharedInstance] addUserAttributesWithUserAttrs:convertNSStringToCDictionary([NSString stringWithUTF8String:userAttrs])];
    }
    
    void _SetPushToken(const char * pushToken)
    {
        [[MetrixPlugin sharedInstance] setPushTokenWithPushToken:[NSString stringWithUTF8String:pushToken]];
    }

    void _SetStore(const char * storeName)
    {
        [[MetrixPlugin sharedInstance] setStoreWithStoreName:[NSString stringWithUTF8String:storeName]];
    }

    void _SetAppSecret(int secretId, long info1, long info2, long info3, long info4)
    {
        [[MetrixPlugin sharedInstance] setAppSecretWithSecretId:secretId info1:info1 info2:info2 info3:info3 info4:info4];
    }
    
    void _SetDefaultTracker(const char * trackerToken)
    {
        [[MetrixPlugin sharedInstance] setDefaultTrackerWithTrackerToken:[NSString stringWithUTF8String:trackerToken]];
    }
    
    void _SetUserIdListener()
    {
        [[MetrixPlugin sharedInstance] setUserIdListener];
    }
    
    void _SetOnDeeplinkResponseListener(bool shouldLaunchDeferredDeeplink)
    {
        [[MetrixPlugin sharedInstance] setOnDeeplinkResponseListener:shouldLaunchDeferredDeeplink];
    }

    void _SetOnAttributionChangedListener()
    {
        [[MetrixPlugin sharedInstance] setOnAttributionChangedListener];
    }
}